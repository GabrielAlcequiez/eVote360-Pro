using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Citizen;
using eVote360_Pro.WebApp.ViewModels.Citizen;

namespace eVote360_Pro.WebApp.Profiles
{
    public class CitizenProfile : Profile
    {
        public CitizenProfile()
        {
            CreateMap<CitizenGetDto, CitizenGetViewModel>();
            CreateMap<CitizenGetDto, CitizenUpdateViewModel>();
            CreateMap<CitizenCreateDto, CitizenCreateViewModel>();
            CreateMap<CitizenUpdateDto, CitizenUpdateViewModel>();

            CreateMap<CitizenCreateViewModel, CitizenCreateDto>();
            CreateMap<CitizenUpdateViewModel, CitizenUpdateDto>();
        }
    }
}
