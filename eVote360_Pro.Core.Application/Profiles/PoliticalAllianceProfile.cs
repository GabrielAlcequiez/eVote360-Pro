using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.PoliticalAlliance;
using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Application.Profiles
{
    public class PoliticalAllianceProfile : Profile
    {
        public PoliticalAllianceProfile()
        {
             CreateMap<PoliticalAlliance, PoliticalAllianceGetDto>()
                .ForMember(dest => dest.RequesterPartyName, opt => opt.MapFrom(src => src.RequesterParty != null ? src.RequesterParty.Name : string.Empty))
                .ForMember(dest => dest.RequesterPartyAcronym, opt => opt.MapFrom(src => src.RequesterParty != null ? src.RequesterParty.Acronym : string.Empty))
                .ForMember(dest => dest.ReceiverPartyName, opt => opt.MapFrom(src => src.ReceiverParty != null ? src.ReceiverParty.Name : string.Empty))
                .ForMember(dest => dest.ReceiverPartyAcronym, opt => opt.MapFrom(src => src.ReceiverParty != null ? src.ReceiverParty.Acronym : string.Empty));
        }
    }
}