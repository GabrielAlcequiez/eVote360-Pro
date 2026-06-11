using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.User;
using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapeo básico de consulta: Entidad User -> DTO de salida
            CreateMap<User, UserGetDto>();

            // Mapeo para actualización a partir del DTO de consulta (utilizado en activación/desactivación)
            CreateMap<UserGetDto, UserUpdateDto>();
        }
    }
}