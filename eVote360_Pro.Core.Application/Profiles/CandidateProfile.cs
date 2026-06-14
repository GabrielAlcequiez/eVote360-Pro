using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Candidate;
using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Application.Profiles
{
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            CreateMap<Candidate, CandidateGetDto>()
                .ForMember(dest => dest.PoliticalPartyName, opt => opt.MapFrom(src => src.PoliticalParty.Name))
                .ForMember(dest => dest.PoliticalPartyAcronym, opt => opt.MapFrom(src => src.PoliticalParty.Acronym))
                .ForMember(dest => dest.PoliticalPartyLogo, opt => opt.MapFrom(src => src.PoliticalParty.Logo));
        }
    }
}