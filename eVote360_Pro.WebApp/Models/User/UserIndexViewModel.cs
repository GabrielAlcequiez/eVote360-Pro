using eVote360_Pro.Core.Application.DTOs.User;

namespace eVote360_Pro.WebApp.Models.User
{
    public class UserIndexViewModel
    {
        public List<UserGetDto> Users { get; set; } = new();
        public bool HasActiveElection { get; set; }
    }
}
