using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.User;
using eVote360_Pro.Core.Application.Helpers;
using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Core.Domain.Entities;
using eVote360_Pro.Core.Domain.Interfaces;

namespace eVote360_Pro.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserGetDto> AddAsync(UserCreateDto dto)
        {
            var hashedPassword = PasswordHelper.HashPassword(dto.Password);

            var user = new User(
                dto.Name,
                dto.LastName,
                dto.Email,
                dto.Username,
                hashedPassword,
                dto.Role
            );

            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserGetDto>(user);
        }

        public async Task DeleteAsync(Guid id)
        {
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
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !user.IsActive) return null;

            if (!PasswordHelper.VerifyPassword(password, user.Password))
                return null;

            return _mapper.Map<UserGetDto>(user);
        }

        public async Task UpdateAsync(UserUpdateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.Id) ?? throw new KeyNotFoundException($"Usuario con ID {dto.Id} no encontrado.");

            var passwordToSave = string.IsNullOrEmpty(dto.Password)
                ? user.Password
                : PasswordHelper.HashPassword(dto.Password);

            user.Update(
                dto.Name,
                dto.LastName,
                dto.Email,
                dto.Username,
                passwordToSave,
                dto.Role,
                dto.IsActive
            );

            await _userRepository.UpdateAsync(user.Id, user);
            await _unitOfWork.CompleteAsync();
        }
    }
}