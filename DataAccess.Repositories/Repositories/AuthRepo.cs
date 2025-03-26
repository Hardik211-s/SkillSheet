
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DataAccess.Entities.Context;
using DataAccess.Entities.Entities;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkillSheetAPI.Models.DTOs;
using System.Data.Common;

namespace DataAccess.Repositories.Repositories
{
    public class AuthRepo : IAuthRepo
    {
        ApplicationDbContext _skillsheetContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthRepo(ApplicationDbContext skillsheetContext, IMapper mapper, IConfiguration config)
        {
            _skillsheetContext = skillsheetContext;
            _mapper = mapper;
            _config = config;
        }


        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of UserDTO objects.</returns>
        public async Task<List<UserDTO>> GetAllUser()
        {
            var allUsers = _skillsheetContext.UserDetails;
            if (allUsers == null)
            {
                return new List<UserDTO>();
            }
            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        /// <summary>
        /// Gets a user by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>A UserDTO object.</returns>
        public async Task<UserDTO> GetUserByUsername(string username)
        {
            var user = await _skillsheetContext.UserDetails.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception(ErrorResource.UserNotExistErrror);
            }
            return _mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="userLoginDTO">The user login DTO.</param>
        /// <returns>A UserDTO object if login is successful, otherwise null.</returns>
        public async Task<UserDTO?> LoginUser(UserLoginDTO userLoginDTO)
        {
            bool isLogged = false;
            var user = await _skillsheetContext.UserDetails.FirstOrDefaultAsync(u => u.Username == userLoginDTO.Username);
            if (user == null)
            {
                throw new Exception(ErrorResource.UserNotExistErrror);
            }
            isLogged = BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.Password);
            if (!isLogged)
            {
                //throw new Exception(.UserExistError);
            }
            return _mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userRegisterDTO">The user register DTO.</param>
        /// <returns>A UserDTO object.</returns>
        public async Task<UserDTO> RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                var existingUser = await _skillsheetContext.UserDetails.FirstOrDefaultAsync(u => u.Username == userRegisterDTO.Username);

                if (existingUser != null) throw new Exception(ErrorResource.UserExistError);

                var userEntity = _mapper.Map<UserDetail>(userRegisterDTO);

                userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password); // Hash Password

                await _skillsheetContext.UserDetails.AddAsync(userEntity);
                await _skillsheetContext.SaveChangesAsync();
                return _mapper.Map<UserDTO>(userEntity);
            }
            catch (DbException)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userRegisterDTO">The user register DTO.</param>
        /// <returns>True if the user is updated, otherwise false.</returns>
        public async Task<bool> UpdateUser(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                var userEntity = await _skillsheetContext.UserDetails.FirstOrDefaultAsync(u => u.Username == userRegisterDTO.Username);
                if (userEntity == null)
                {
                    throw new Exception("User is not register");
                }
                userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password); // Hash Password
                userEntity.Username = userRegisterDTO.Username;
                userEntity.Email = userRegisterDTO.Email;
                _skillsheetContext.UserDetails.Update(userEntity);
                await _skillsheetContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                throw new Exception("An error occurred while update user in database.");
            }
        }

        public async Task<bool> CheckUserPassword(UserRegisterDTO userData)
        {

            var userEntity = await _skillsheetContext.UserDetails.FirstOrDefaultAsync(u => u.Username == userData.Username);
            if (userEntity == null)
            {
                throw new Exception(ErrorResource.UserNotExistErrror);
            }
            bool isLogged = BCrypt.Net.BCrypt.Verify(userData.Password, userEntity.Password);
            return isLogged;

        }

        /// <summary>
        /// Deletes a user by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>True if the user is deleted, otherwise false.</returns>
        public async Task<bool> DeleteUser(string username)
        {
            try
            {


                var userEntity = await _skillsheetContext.UserDetails.FirstOrDefaultAsync(u => u.Username == username);
                if (userEntity != null && userEntity.Role == GeneralResource.Admin)
                {
                    throw new Exception(ErrorResource.NotDeleteAdminError);
                }

                if (userEntity == null)
                {
                    throw new Exception(ErrorResource.UserNotExistErrror);
                }
                _skillsheetContext.UserDetails.Remove(userEntity);
                await _skillsheetContext.SaveChangesAsync();
                return true;
            }
            catch (DbException)
            {
                throw new Exception("An error occurred while delete user in database.");
            }
        }




        /// <summary>
        /// Generates a JWT token for a user.
        /// </summary>
        /// <param name="user">The user DTO.</param>
        /// <returns>A JWT token string.</returns>
        public string GenerateJwtToken(UserDTO user)
        {
            var claims = new[]
            {
                new Claim(GeneralResource.UserId, Convert.ToString(user.Userid)),
                new Claim(GeneralResource.Email, user.Email),
                new Claim(GeneralResource.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(GeneralResource.Role, user.Role),
            };
            var secretKey = _config[GeneralResource.JwtKey];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(GeneralResource.JwtKey, ErrorResource.JwtError);
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

