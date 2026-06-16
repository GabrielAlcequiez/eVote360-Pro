using eVote360_Pro.Core.Application.DTOs.Shared;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest request);
    }
}