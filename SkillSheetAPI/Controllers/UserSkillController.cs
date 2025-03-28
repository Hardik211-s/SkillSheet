using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Resources;
using Microsoft.AspNetCore.Authorization;
namespace SkillSheetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSkillController : Controller
    {
        IUserSkillService _userSkillService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSkillController"/> class.
        /// </summary>
        /// <param name="userSkillService">The user skill service.</param>

        public UserSkillController(IUserSkillService userSkillService)
        {
            _userSkillService = userSkillService;
        }


        /// <summary>
        /// Gets all user skills.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of user skills.</returns>

        [HttpGet]
        public async Task<IActionResult> GetUserSkill()
        {
            try
            {
                var userSkill =await _userSkillService.AllUserSkillService();
                return Ok(new { userSkill });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets user skills by user ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user skills.</returns>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserSkill(int id)
        {
            try
            {
                var userSkill = await _userSkillService.GetUserSkillService(id);
                return Ok(new { userSkill });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new user skill.
        /// </summary>
        /// <param name="userSkillDTO">The user skill DTO.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost]
        [Authorize(Roles = "User")] 
        public async Task<IActionResult> AddUserSkill(UserSkillDTO userSkillDTO)
        {
            try
            {
                var userSkill = await _userSkillService.AddUserSkillService(userSkillDTO);
                if (userSkill == null) {
                    return BadRequest(ErrorResource.GeneralError);
                }
                return Ok(new { message = "User skill added!" });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Edits an existing user skill.
        /// </summary>
        /// <param name="userSkillDTO">The user skill DTO.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPatch]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> EditUserSkill(DbUserSkillDTO userSkillDTO)
        {
            try
            { 
                var userSkill = await _userSkillService.EditUserSkillService(userSkillDTO);
                if (userSkill == null)
                {
                    return BadRequest(ErrorResource.GeneralError);
                }
                return Ok(new { message = "User skill edited!" });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a user skill by ID.
        /// </summary>
        /// <param name="id">The user skill ID.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> DeleteUserSkill(int id)
        {
            try
            {
                var userSkill = await _userSkillService.DeleteUserSkillService(id);
                if (userSkill == null)
                {
                    return BadRequest(ErrorResource.GeneralError);
                }
                return Ok(new { message="User skill deleted!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
