using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Ballot;
using eVote360_Pro.WebApp.Models.Ballot;

namespace eVote360_Pro.WebApp.Profiles
{
    public class BallotProfile : Profile
    {
        public BallotProfile()
        {
            CreateMap<OfficeWithCandidatesDto, OfficeViewModel>()
                .ForMember(dest => dest.HasSelection, opt => opt.Ignore());

            CreateMap<CandidateOptionDto, CandidateOptionViewModel>();
        }
    }
}
