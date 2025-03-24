 
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;

namespace SkillSheetAPI.Services.Services
{
    public class DashboardService : IDashboardService
    {
        IUserSkillService _skillService;
        IAuthService _authService;
        IUserDetailService _userDetailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="userSkillService">The user skill service.</param>
        /// <param name="authService">The authentication service.</param>
        /// <param name="userDetailService">The user detail service.</param>
        public DashboardService(IUserSkillService userSkillService, IAuthService authService,IUserDetailService userDetailService) {
            _authService = authService;
            _skillService = userSkillService;
            _userDetailService = userDetailService;
        }

        /// <summary>
        /// Gets the dashboard data.
        /// </summary>
        /// <returns>A <see cref="DashboardDataDTO"/> containing the dashboard data.</returns>
        public async Task<DashboardDataDTO>GetDashboardData()
        {
            //Call other servoce here for data
            DashboardDataDTO dashboardDataDTO = new DashboardDataDTO();
            dashboardDataDTO.TotalUser= (await _authService.GetAllUserService()).Count();
            dashboardDataDTO.TotalSkill=(await _skillService.AllUserSkillService()).Select(x => x.Skill).Distinct().Count();
            dashboardDataDTO.UserAllData=await _skillService.AllUserSkillService();
            dashboardDataDTO.AllUserDetail = await _userDetailService.GetAllUserService();
            dashboardDataDTO.ExperienceAVG =Math.Truncate( dashboardDataDTO.UserAllData.Average(x => x.Experience));
            return dashboardDataDTO;
            }
    }
}
