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
                .ForMember(dest => dest.CandidateFullName,
                    opt => opt.MapFrom(src => $"{src.Candidate.Name} {src.Candidate.LastName}"));
        }
    }
}
