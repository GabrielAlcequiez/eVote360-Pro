using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Election;
using eVote360_Pro.WebApp.Models.Election;

namespace eVote360_Pro.WebApp.Profiles
{
    public class ElectionProfile : Profile
    {
        public ElectionProfile()
        {
            CreateMap<ElectionListDto, ElectionGetViewModel>();
            CreateMap<ElectionCreateViewModel, ElectionCreateDto>();

            CreateMap<OfficeResultDto, OfficeResultViewModel>();
            CreateMap<CandidateResultDto, CandidateResultViewModel>();
        }
    }
}
