using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.User;
using eVote360_Pro.WebApp.Models;

namespace eVote360_Pro.WebApp.Profiles
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<UserCreateViewModel, UserCreateDto>();
            CreateMap<UserUpdateViewModel, UserUpdateDto>();
            CreateMap<UserGetDto, UserUpdateViewModel>();
        }
    }
}