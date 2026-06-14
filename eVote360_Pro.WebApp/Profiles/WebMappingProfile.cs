using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.PartyLeader;
using eVote360_Pro.Core.Application.DTOs.User;
using eVote360_Pro.WebApp.Models;
using eVote360_Pro.WebApp.Models.PartyLeader;
using eVote360_Pro.WebApp.Models.User;

namespace eVote360_Pro.WebApp.Profiles
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<UserCreateViewModel, UserCreateDto>();
            CreateMap<UserUpdateViewModel, UserUpdateDto>();
            CreateMap<UserGetDto, UserUpdateViewModel>();
            CreateMap<PartyLeaderCreateViewModel, PartyLeaderCreateDto>();
        }
    }
}