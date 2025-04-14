using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Resources;
namespace SkillSheetAPI.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        IDashboardService _dashboardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="dashboardService">The dashboard service.</param>

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;

        }

        /// <summary>
        /// Gets the dashboard data.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the dashboard data.</returns>

        [HttpGet]
        public async Task<IActionResult> DashboardData()
        {
            try
            {
                var data =await _dashboardService.GetDashboardData();
                return Ok(new { message = GeneralResource.DashboardDataSuccess, data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
         
    }
}
