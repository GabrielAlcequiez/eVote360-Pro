using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.Citizen;
using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Application.Profiles
{
    public class CitizenProfile : Profile
    {
        public CitizenProfile()
        {
            CreateMap<Citizen, CitizenGetDto>();
            CreateMap<CitizenGetDto, CitizenUpdateDto>();
        }        
    }
}