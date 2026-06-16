using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.CandidateOfficeAssignment;
using eVote360_Pro.WebApp.Models.CandidateOfficeAssignment;

namespace eVote360_Pro.WebApp.Profiles
{
    public class CandidateOfficeAssignmentProfile : Profile
    {
        public CandidateOfficeAssignmentProfile()
        {
            CreateMap<CandidateOfficeAssignmentGetDto, CandidateOfficeAssignmentGetViewModel>();
            CreateMap<CandidateOfficeAssignmentCreateViewModel, CandidateOfficeAssignmentCreateDto>();
        }
    }
}
