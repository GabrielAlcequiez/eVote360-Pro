// Capa: Core.Application → Profiles
// Propósito: Le dice a AutoMapper cómo convertir la entidad PartyLeader
//            hacia los DTOs. Aquí mapeamos los campos que vienen de las
//            propiedades de navegación (User y PoliticalParty).

using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.PartyLeader;
using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Application.Profiles
{
    public class PartyLeaderProfile : Profile
    {
        public PartyLeaderProfile()
        {
            CreateMap<PartyLeader, PartyLeaderGetDto>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => $"{src.User.Name} {src.User.LastName}"))
                .ForMember(dest => dest.UserEmail,
                    opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.UserUsername,
                    opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.UserIsActive,
                    opt => opt.MapFrom(src => src.User.IsActive))
                .ForMember(dest => dest.PoliticalPartyName,
                    opt => opt.MapFrom(src => src.PoliticalParty.Name))
                .ForMember(dest => dest.PoliticalPartyAcronym,
                    opt => opt.MapFrom(src => src.PoliticalParty.Acronym))
                .ForMember(dest => dest.PoliticalPartyIsActive,
                    opt => opt.MapFrom(src => src.PoliticalParty.IsActive));
        }
    }
}