using AutoMapper;
using eVote360_Pro.Core.Application.DTOs.ElectedOffice;
using eVote360_Pro.Core.Domain.Entities;

namespace eVote360_Pro.Core.Application.Profiles
{
    public class ElectedOfficeProfile : Profile
    {
        public ElectedOfficeProfile()
        {
            CreateMap<ElectedOffice, ElectedOfficeGetDto>();
        }
    }
}