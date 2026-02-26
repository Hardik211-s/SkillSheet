using SkillSheetAPI.Models.DTOs;

namespace SkillSheetAPI.Services.Interfaces
{
    public interface IIssueService
    {
        Task<bool> CreateIssueAsync(CreateIssueDTO dto);
    }
}
