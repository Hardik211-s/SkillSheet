
using AutoMapper;

using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;

namespace SkillSheetAPI.Services.Services
{
    public class UserDetailService : IUserDetailService
    {

        IUserDetailRepo _userDetailRepo { get; set; }
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserDetailService(IUserDetailRepo userDetailRepo, IMapper mapper, IConfiguration configuration)
        {
            this._userDetailRepo = userDetailRepo;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        #region GetAllUserService
        /// <summary>
        /// Gets all user details asynchronously.
        /// </summary>
        /// <returns>A list of user details.</returns>
        public async Task<List<DbUserDetailDTO>> GetAllUserService()
        {
            var userModel = await _userDetailRepo.GetAllUserDetail();
            return _mapper.Map<List<DbUserDetailDTO>>(userModel);
        }
        #endregion

        #region AddUserDetailService
        /// <summary>
        /// Adds a new user detail asynchronously.
        /// </summary>
        /// <param name="userDetailDto">The user detail DTO.</param>
        /// <returns>A boolean indicating success.</returns>
        public async Task<bool> AddUserDetailService(UserDetailDTO userDetailDto)
        {
            string imageURL = await Upload(userDetailDto.Photo);
            var userModel = await _userDetailRepo.AddUserDetail(userDetailDto, imageURL);
            return true;
        }
        #endregion

        #region GetUserDetailService
        /// <summary>
        /// Gets user detail by ID asynchronously.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user detail.</returns>
        public async Task<DbUserDetailDTO> GetUserDetailService(int id)
        {
            var userDetail = await _userDetailRepo.GetUserDetailById(id);
            userDetail.Age = CountAge(userDetail.Birthdate);
            return userDetail;
        }
        #endregion

        #region EditUserDetailService
        /// <summary>
        /// Edits an existing user detail asynchronously.
        /// </summary>
        /// <param name="userDetailDto">The user detail DTO.</param>
        /// <returns>A boolean indicating success.</returns>
        public async Task<bool> EditUserDetailService(UserDetailDTO userDetailDto)
        {
            string imageURL = "";
            if (userDetailDto.Photo != null)
            {
                imageURL = await Upload(userDetailDto.Photo);
            }
            var userDetail = await _userDetailRepo.EditUserDetail(userDetailDto, imageURL);
            if (userDetail == null)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region DeleteUserDetailService
        /// <summary>
        /// Deletes a user detail by username asynchronously.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>A boolean indicating success.</returns>
        public async Task<bool> DeleteUserDetailService(string username)
        {
            bool isDeleted = await _userDetailRepo.DeleteUserDetail(username);
            return isDeleted;
        }
        #endregion

        #region Upload
        /// <summary>
        /// Uploads an image asynchronously.
        /// </summary>
        /// <param name="file">The image file.</param>
        /// <returns>The URL of the uploaded image.</returns>
        public async Task<string> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new Exception("No file uploaded.");
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new Exception("Invalid file type. Allowed types: jpg, jpeg, png, gif");
                }

                var maxFileSize = 10485760; // Get max file size from configuration
                if (file.Length > maxFileSize)
                {
                    Console.WriteLine(maxFileSize + "      " + file.Length);
                    throw new Exception($"File size exceeds the limit of {maxFileSize} bytes.");
                }

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var fullPath = Path.Combine(pathToSave, uniqueFileName);
                string dbPath = Path.Combine(folderName, uniqueFileName).Replace("\\", "/");

                using (var stream = file.OpenReadStream())
                {
                    using (var image = System.Drawing.Image.FromStream(stream))
                    {
                        image.Save(fullPath);
                    }
                }

                return dbPath;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region CountAge
        /// <summary>
        /// Counts the age based on the date of birth.
        /// </summary>
        /// <param name="dob">The date of birth.</param>
        /// <returns>The calculated age.</returns>
        public int CountAge(DateOnly dob)
        {
            DateTime current = DateTime.Now;
            int currentDate = current.Day;
            int currentMonth = current.Month;
            int currentYear = current.Year;

            int age = currentYear - dob.Year;
            if (dob.Month > currentMonth)
            {
                age--;
            }
            else if (dob.Month == currentMonth)
            {
                if (dob.Day > currentDate)
                {
                    age--;
                }
            }
            return age;
        }
        #endregion

    }
}
