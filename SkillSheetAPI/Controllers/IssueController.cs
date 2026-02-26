using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;

namespace SkillSheetAPI.Controllers
{
    [ApiController]
    [Route("api/issue")]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateIssueDTO dto)
        {
            try
            {
                bool created = await _issueService.CreateIssueAsync(dto);
                if (created) return Ok(new { message = "Issue created successfully" });
                return BadRequest(new { message = "Failed to create issue" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
