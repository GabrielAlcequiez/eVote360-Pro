using eVote360_Pro.Core.Application.DTOs.Candidate;
using eVote360_Pro.Core.Application.DTOs.CandidateOfficeAssignment;
using eVote360_Pro.Core.Application.DTOs.ElectedOffice;

namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface ICandidateOfficeAssignmentService
    {
        Task<CandidateOfficeAssignmentGetDto?> AddAssignment(CandidateOfficeAssignmentCreateDto dto, Guid userId);
        Task<bool> DeleteAssignment(Guid id, Guid userId);
        Task<IReadOnlyList<CandidateOfficeAssignmentGetDto>> GetAllAssignmentsAsync(Guid userId);
        Task<CandidateOfficeAssignmentGetDto?> GetById(Guid id, Guid userId);
        Task<List<CandidateGetDto>> GetAvailableCandidatesAsync(Guid partyId);
        Task<List<ElectedOfficeGetDto>> GetAvailableOfficesAsync(Guid partyId);
    }
}