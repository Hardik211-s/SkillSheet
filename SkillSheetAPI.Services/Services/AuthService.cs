
using AutoMapper;
using DataAccess.Repositories.Interfaces;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Services.Resource;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace SkillSheetAPI.Services.Services
{
    public class AuthService : IAuthService
    {
        IAuthRepo _authRepo { get; set; }
        IEmailService _emailService { get; set; }
        private readonly IConfiguration _config;

        private readonly IMapper _mapper;

        public AuthService(IAuthRepo authRepo, IMapper mapper, IEmailService emailService, IConfiguration config)
        {
            this._authRepo = authRepo;
            this._mapper = mapper;
            _emailService = emailService;
            _config = config;
        }
      

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of UserDTO objects.</returns>
        public async Task<List<UserDTO>> GetAllUserService()
        {
            var allUser = await _authRepo.GetAllUser();
            return allUser;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userDto">The user register DTO.</param>
        /// <returns>A UserDTO object.</returns>
        public async Task<UserDTO> RegisterUserService(UserRegisterDTO userDto)
        {
            try
            {
                
                var userData = await _authRepo.RegisterUser(userDto);
                if (userData != null)
                {
                    await _emailService.SendEmail(userData.Username, userDto.Password, userData.Email, "Created");
                }
                return userData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="userDto">The user login DTO.</param>
        /// <returns>A tuple containing a UserDTO object and a JWT token string.</returns>
        public async Task<(UserDTO, string)> LoginUserService(UserLoginDTO userDto)
        {
            try
            {
                var user = await _authRepo.LoginUser(userDto);
                string token =GenerateJwtToken(user);
                return (user, token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userDto">The user register DTO.</param>
        /// <returns>True if the user is updated, otherwise false.</returns>
        public async Task<bool> UpdateUserService(UserRegisterDTO userDto)
        {
            try
            {
                bool isUpdated = await _authRepo.UpdateUser(userDto);
                if (isUpdated)
                {
                    //await _emailService.SendEmail(userDto.Username, userDto.Password, userDto.Email, "Edited");
                }
                return isUpdated;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>True if the user is deleted, otherwise false.</returns>
        public async Task<bool> ChangePasswordService(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                UserRegisterDTO userdata = _mapper.Map<UserRegisterDTO>(changePasswordDTO);
                bool checkPassword = await _authRepo.CheckUserPassword(userdata);

                if (!checkPassword)
                {
                    throw new Exception("User password is wrong");
                }

                userdata.Password = changePasswordDTO.NewPassword;

                bool isUpdated = await _authRepo.UpdateUser(userdata);
                if (isUpdated)
                {
                    //await _emailService.SendEmail(userdata.Username, userdata.Password, userdata.Email, "Edited");
                }
                return isUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        /// <summary>
        /// Deletes a user by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>True if the user is deleted, otherwise false.</returns>
        public async Task<bool> DeleteUserService(string username)
        {
            try
            {

                var user = await _authRepo.GetUserByUsername(username);

                bool isDeleted = await _authRepo.DeleteUser(username);
                return isDeleted;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Generates a JWT token for a user.
        /// </summary>
        /// <param name="user">The user DTO.</param>
        /// <returns>A JWT token string.</returns>
        public string GenerateJwtToken(UserDTO user)
        {
            var claims =new[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(GeneralResource.UserId, Convert.ToString(user.Userid)),
                new Claim(GeneralResource.Email, user.Email),
                new Claim(GeneralResource.Name, user.Username),
                new Claim("Role", user.Role),
            };

           
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddHours(10),
           signingCredentials: creds
       );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
