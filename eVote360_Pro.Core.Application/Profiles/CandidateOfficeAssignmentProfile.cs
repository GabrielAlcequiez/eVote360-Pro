using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.CandidateOfficeAssignment;
using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Application.Profiles
{
    public class CandidateOfficeAssignmentProfile : Profile
    {
        public CandidateOfficeAssignmentProfile()
        {
            CreateMap<CandidateOfficeAssignment, CandidateOfficeAssignmentGetDto>()
        .ForMember(
            dest => dest.CandidateFullName,
            opt => opt.MapFrom(src =>
                $"{src.Candidate.Name} {src.Candidate.LastName}"))
        .ForMember(
            dest => dest.CandidatePhoto,
            opt => opt.MapFrom(src => src.Candidate.Photo))
        .ForMember(
            dest => dest.ElectedOfficeName,
            opt => opt.MapFrom(src => src.ElectedOffice.Name))
        .ForMember(
            dest => dest.PoliticalPartyName,
            opt => opt.MapFrom(src => src.PoliticalParty.Name))
        .ForMember(
            dest => dest.PoliticalPartyAcronym,
            opt => opt.MapFrom(src => src.PoliticalParty.Acronym));
        }
    }
}
