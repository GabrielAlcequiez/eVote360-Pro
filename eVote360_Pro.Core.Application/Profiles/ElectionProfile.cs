using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Election;
using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Application.Profiles
{
    public class ElectionProfile : Profile
    {
        public ElectionProfile()
        {
            CreateMap<Election, ElectionGetDto>();
            CreateMap<ElectionGetDto, ElectionUpdateDto>();
            CreateMap<Election, ElectionListDto>();
        }
    }
}
