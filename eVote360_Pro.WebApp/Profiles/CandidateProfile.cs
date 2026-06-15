using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Candidate;
using eVote360_Pro.WebApp.Models.Candidate;

namespace eVote360_Pro.WebApp.Profiles
{
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            CreateMap<CandidateGetDto, CandidateGetViewModel>();
            CreateMap<CandidateGetDto, CandidateUpdateViewModel>();
            CreateMap<CandidateGetDto, CandidateUpdateDto>();
            CreateMap<CandidateCreateDto, CandidateCreateViewModel>();
            CreateMap<CandidateUpdateDto, CandidateUpdateViewModel>();
            CreateMap<CandidateCreateViewModel, CandidateCreateDto>();
            CreateMap<CandidateUpdateViewModel, CandidateUpdateDto>();
        }
    }
}