using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces; 
using SkillSheetAPI.Resources;
namespace SkillSheetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDetailController : Controller
    {

        IUserDetailService _userDetailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetailController"/> class.
        /// </summary>
        /// <param name="userDetailService">The user detail service.</param>

        public UserDetailController(IUserDetailService userDetailService)
        {
            _userDetailService = userDetailService;
        }

        /// <summary>
        /// Gets all user details.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of user details.</returns>
        [HttpGet("AllUserDetail")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllUserDetail()
        {
            try
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                var allUserDetail =await _userDetailService.GetAllUserService();
                return Ok(new { message = GeneralResource.AllUserFound, allUserDetail });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new user detail.
        /// </summary>
        /// <param name="userDetailDto">The user detail DTO.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>

        [HttpPost("AddUserDetail")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> AddUserDetail([FromForm] UserDetailDTO userDetailDto)
        {
            try
            {

                bool user =await _userDetailService.AddUserDetailService(userDetailDto);
                if (user)
                {
                    return Ok(new { message = GeneralResource.UserAddSuccess });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Edits an existing user detail.
        /// </summary>
        /// <param name="userDetailDto">The user detail DTO.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>

        [HttpPatch("EditUserDetail")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> EditUserDetail([FromForm] UserDetailDTO userDetailDto)
        { 
            try
            { 

                bool user =await _userDetailService.EditUserDetailService(userDetailDto);
                if (user)
                {
                    return Ok(new { message = GeneralResource.UserEdit });
                }
                return BadRequest(new { message = GeneralResource.UserNotEdit });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets user detail by ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user detail.</returns>
        [HttpGet("UserDetailById/{id}")]
        //[Authorize(Roles = "User")]

        public async Task<IActionResult> UserDetailById(int id)
        {

            try
            { 
                
                var userDetail =await _userDetailService.GetUserDetailService(id);
                if (userDetail == null)
                {
                    return NotFound(new { message = GeneralResource.UserNotFound });
                }
                return Ok(new { message = GeneralResource.AllUserFound, userDetail });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Deletes a user detail by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("DeleteUserDetail")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> DeleteUserDetail([FromBody] string username)
        {

            try
            {
                bool isDeleted =await _userDetailService.DeleteUserDetailService(username);
                if (isDeleted)
                {
                    return Ok(new { message = GeneralResource.UserDelete });
                }
                return Ok(new { message = GeneralResource.UserNotDelete});

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
       



    }
}
