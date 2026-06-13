using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.PoliticalParty;
using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Application.Profiles
{
    public class PoliticalPartyProfile : Profile
    {
        public PoliticalPartyProfile()
        {
            CreateMap<PoliticalParty, PoliticalPartyGetDto>()
                .ForMember(dest => dest.LeaderFullName, opt => opt.MapFrom(src =>
                    src.PartyLeader != null && src.PartyLeader.User != null
                    ? $"{src.PartyLeader.User.Name} {src.PartyLeader.User.LastName}"
                    : null));

            CreateMap<PoliticalPartyGetDto, PoliticalPartyUpdateDto>();
        }
    }
}
