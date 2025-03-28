using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Resources;
using Microsoft.AspNetCore.Authorization;

namespace SkillSheetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>

        public AuthController(IAuthService authService)
        {
            _authService = authService;

        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of users.</returns>

        [HttpGet("allUser")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                
                var allUsers =await _authService.GetAllUserService();
                return Ok(new { message = GeneralResource.UserDataFoundSuccess, allUsers });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userDto">The user registration DTO.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userDto)
        {
            try
            {
                var user =await _authService.RegisterUserService(userDto);
                
                return Ok(new { message = "User register successfully !", user});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="userDto">The user login DTO.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user data and token.</returns>

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userDto)
        {
            Console.WriteLine(userDto.Role);
            try
            {
                var (userData, token) =await _authService.LoginUserService(userDto);
                return Ok(new { message = GeneralResource.UserLogin, userData, token });

            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });

            }

        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userDto">The user registration DTO.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>

        //[Authorize]
        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserRegisterDTO userDto)
        {

            try
            { 
                bool isUpdate=await _authService.UpdateUserService(userDto);
                
                return Ok(new { message =GeneralResource.UserEdit});
            
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Changes the password of a user.
        /// </summary>
        /// <param name="changePasswordDTO">The change password DTO.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>

        //[Authorize]
        [HttpPatch("Password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {

            try
            {
                bool isUpdate = await _authService.ChangePasswordService(changePasswordDTO);
                if (isUpdate == false)
                {
                    return NotFound(new { message =GeneralResource.UserNotFound });
                }
                return Ok(new { message =GeneralResource.UserChangePassword });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>

        [HttpDelete("Delete/{username}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteUser(string username)
        {

            try
            {
                bool isDeleted =await _authService.DeleteUserService(username);
                return Ok(new { message = GeneralResource.UserDelete });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
 


    }
}
