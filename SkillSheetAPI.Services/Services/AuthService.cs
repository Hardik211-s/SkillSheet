
using AutoMapper;
using DataAccess.Repositories.Interfaces;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI;

namespace SkillSheetAPI.Services.Services
{
    public class AuthService :IAuthService
    {
        IAuthRepo _authRepo { get; set; }
        IEmailService _emailService { get; set; }
        private readonly IMapper _mapper;

        public AuthService(IAuthRepo authRepo,IMapper mapper, IEmailService emailService)
        {
            this._authRepo = authRepo;
            this._mapper = mapper;
            _emailService = emailService;
        }
        #region Public Methods

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of UserDTO objects.</returns>
        public async Task<List<UserDTO>> GetAllUserService()
        {
            var userModel = await _authRepo.GetAllUser();
            return userModel;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userDto">The user register DTO.</param>
        /// <returns>A UserDTO object.</returns>
        public async Task<UserDTO> RegisterUserService(UserRegisterDTO userDto)
        {
            var userModel = await _authRepo.RegisterUser(userDto);
            if ( userModel!=null)
            {
                await _emailService.SendEmail(userModel.Username, userDto.Password, userModel.Email, "Created");
            }
            return userModel;
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="userDto">The user login DTO.</param>
        /// <returns>A tuple containing a UserDTO object and a JWT token string.</returns>
        public async Task<(UserDTO, string)> LoginUserService(UserLoginDTO userDto)
        {
            var user = await _authRepo.LoginUser(userDto);
            string token = string.Empty;
            if (user != null)
            {
                token = _authRepo.GenerateJwtToken(user);
            }
            return (user, token);
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userDto">The user register DTO.</param>
        /// <returns>True if the user is updated, otherwise false.</returns>
        public async Task<bool> UpdateUserService(UserRegisterDTO userDto)
        {
            bool isUpdated = await _authRepo.UpdateUser(userDto);
            if (isUpdated)
            {
               await _emailService.SendEmail(userDto.Username, userDto.Password, userDto.Email, "Edited");
            }
            return isUpdated;
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>True if the user is deleted, otherwise false.</returns>
        public async Task<bool> ChangePasswordService(ChangePasswordDTO changePasswordDTO)
        {
            UserRegisterDTO userdata = _mapper.Map<UserRegisterDTO>(changePasswordDTO);
            bool  checkPassword=await  _authRepo.CheckUserPassword(userdata);

            if (!checkPassword)
            {
                throw new Exception();
            }

            userdata.Password = changePasswordDTO.NewPassword;

            bool isUpdated = await _authRepo.UpdateUser(userdata);
            if (isUpdated)
            {
                await _emailService.SendEmail(userdata.Username, userdata.Password, userdata.Email, "Edited");
            }
            return isUpdated;
        }

        

        /// <summary>
        /// Deletes a user by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>True if the user is deleted, otherwise false.</returns>
        public async Task<bool> DeleteUserService(string username)
        {
            var user = await _authRepo.GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }
            bool isDeleted = await _authRepo.DeleteUser(username);
            return isDeleted;
        }

        #endregion

    }
}
