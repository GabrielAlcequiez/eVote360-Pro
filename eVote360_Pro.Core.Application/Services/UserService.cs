using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.User;
using eVote360_Pro.Core.Application.Helpers;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;
using eVote360_Pro.Core.Domain.Common.Enums;
using FluentValidation;
using eVote360_Pro.Core.Application.Validators.User;

namespace eVote360_Pro.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IElectionRepository _electionRepository;
        private readonly IBaseRepository<PartyLeader> _partyLeaderRepository;
        //validators
        private readonly IValidator<UserCreateDto> _createValidator;
        private readonly IValidator<UserUpdateDto> _updateValidator;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IElectionRepository electionRepository,
            IBaseRepository<PartyLeader> partyLeaderRepository,
            IValidator<UserCreateDto> createValidator,
            IValidator<UserUpdateDto> updateValidator
        )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _electionRepository = electionRepository;
            _partyLeaderRepository = partyLeaderRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;

        }

        public async Task<UserGetDto> AddAsync(UserCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            await _createValidator.ValidateAndThrowAsync(dto);

            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");


            var cleanUsername = dto.Username.Trim();
            var existingByUsername = await _userRepository.GetByUsernameAsync(cleanUsername);
            if (existingByUsername != null)
            {
                throw new InvalidOperationException("Ya existe un usuario registrado con este nombre de usuario.");
            }

            var cleanEmail = dto.Email.Trim();
            var existingByEmail = await _userRepository.GetByEmailAsync(cleanEmail);
            if (existingByEmail != null)
            {
                throw new InvalidOperationException("Ya existe un usuario registrado con este correo electrónico.");
            }

            var hashedPassword = PasswordHelper.HashPassword(dto.Password);

            var user = new User(
                dto.Name,
                dto.LastName,
                cleanEmail,
                cleanUsername,
                hashedPassword,
                dto.Role
            );

            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserGetDto>(user);
        }

        public async Task DeleteAsync(Guid id, Guid currentUserId)
        {
            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            if (id == currentUserId)
                throw new InvalidOperationException("No puedes desactivarte a ti mismo.");

            var target = await _userRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            if (target.IsActive && target.Role == Role.Administrator)
            {
                var allUsers = await _userRepository.GetAllAsync();
                var activeAdminCount = allUsers.Count(u => u.IsActive && u.Role == Role.Administrator);
                if (activeAdminCount <= 1)
                    throw new InvalidOperationException("No se puede desactivar al único administrador activo del sistema.");
            }

            _ = await _userRepository.SoftDeleteAsync(id) ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<UserGetDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserGetDto>>(users);
        }

        public async Task<UserGetDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return _mapper.Map<UserGetDto>(user);
        }

        public async Task<UserGetDto?> LoginAsync(string username, string password)
        {
            var cleanUsername = username.Trim();
            var user = await _userRepository.GetByUsernameAsync(cleanUsername);
            if (user == null || !user.IsActive) return null;

            if (!PasswordHelper.VerifyPassword(password, user.Password))
                return null;

            return _mapper.Map<UserGetDto>(user);
        }

        public async Task UpdateAsync(UserUpdateDto dto)
        {
             ArgumentNullException.ThrowIfNull(dto);
            await _updateValidator.ValidateAndThrowAsync(dto);
            
            if (await _electionRepository.ValidateNoActiveElectionAsync())
                throw new InvalidOperationException("No se puede realizar esta operación mientras exista una elección activa");

            var user = await _userRepository.GetByIdAsync(dto.Id) ?? throw new KeyNotFoundException($"Usuario con ID {dto.Id} no encontrado.");

            var cleanUsername = dto.Username.Trim();
            var existingByUsername = await _userRepository.GetByUsernameAsync(cleanUsername);
            if (existingByUsername != null && existingByUsername.Id != dto.Id)
            {
                throw new InvalidOperationException("Ya existe un usuario registrado con este nombre de usuario.");
            }

            var cleanEmail = dto.Email.Trim();
            var existingByEmail = await _userRepository.GetByEmailAsync(cleanEmail);
            if (existingByEmail != null && existingByEmail.Id != dto.Id)
            {
                throw new InvalidOperationException("Ya existe un usuario registrado con este correo electrónico.");
            }

            if (user.IsActive && user.Role == Role.Administrator && (!dto.IsActive || dto.Role != Role.Administrator))
            {
                var allUsers = await _userRepository.GetAllAsync();
                var activeAdminCount = allUsers.Count(u => u.IsActive && u.Role == Role.Administrator);
                if (activeAdminCount <= 1)
                    throw new InvalidOperationException("No se puede modificar al único administrador activo del sistema.");
            }

            if (user.Role == Role.PoliticalLeader && dto.Role == Role.Administrator)
            {
                var assignedParty = await _partyLeaderRepository.GetByIdAsync(user.Id);
                if (assignedParty != null)
                {
                    throw new InvalidOperationException("No se puede cambiar el rol de este usuario porque tiene un partido político asignado como dirigente.");
                }
            }

            var passwordToSave = string.IsNullOrEmpty(dto.Password)
                ? user.Password
                : PasswordHelper.HashPassword(dto.Password);

            user.Update(
                dto.Name,
                dto.LastName,
                cleanEmail,
                cleanUsername,
                passwordToSave,
                dto.Role,
                dto.IsActive
            );

            await _userRepository.UpdateAsync(user.Id, user);
            await _unitOfWork.CompleteAsync();
        }
    }
}