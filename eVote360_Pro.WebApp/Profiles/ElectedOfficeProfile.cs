using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.ElectedOffice;
using eVote360_Pro.WebApp.Models.ElectedOffice;

namespace eVote360_Pro.WebApp.Profiles
{
    public class ElectedOfficeProfile : Profile
    {
        public ElectedOfficeProfile()
        {
            CreateMap<ElectedOfficeGetDto, ElectedOfficeGetViewModel>();
            CreateMap<ElectedOfficeGetDto, ElectedOfficeUpdateViewModel>();
            CreateMap<ElectedOfficeCreateDto, ElectedOfficeCreateViewModel>();
            CreateMap<ElectedOfficeUpdateDto, ElectedOfficeUpdateViewModel>();

            CreateMap<ElectedOfficeCreateViewModel, ElectedOfficeCreateDto>();
            CreateMap<ElectedOfficeUpdateViewModel, ElectedOfficeUpdateDto>();

        }
    }
}