 
using SkillSheetAPI.Models.DTOs;

namespace SkillSheetAPI.Services.Interfaces
{
    public interface IDashboardService
    {
        public Task<DashboardDataDTO> GetDashboardData();
    }
}
