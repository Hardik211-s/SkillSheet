
using AutoMapper;
using DataAccess.Entities.Context;
using SkillSheetAPI.Models.DTOs;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repositories.Resource;
using System.Data.Common;

namespace DataAccess.Repositories.Repositories
{
    public class UserDetailRepo : IUserDetailRepo
    {
        ApplicationDbContext _skillsheetContext;
        private readonly IMapper _mapper;

        public UserDetailRepo(ApplicationDbContext skillsheetContext, IMapper mapper)
        {
            _skillsheetContext = skillsheetContext;
            _mapper = mapper;
        }

        #region GetAllUserDetailAsync
        /// <summary>
        /// Gets all user details asynchronously.
        /// </summary>
        /// <returns>A list of user details.</returns>
        public async Task<List<DbUserDetailDTO>> GetAllUserDetail()
        {
            var allUserDetail =  _skillsheetContext.UserDetails;
            if (allUserDetail == null) throw new Exception(ErrorResource.UserDetailNotExistError);
            return _mapper.Map<List<DbUserDetailDTO>>(allUserDetail);
        }
        #endregion

        #region GetUserDetailByIdAsync
        /// <summary>
        /// Gets user detail by ID asynchronously.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user detail.</returns>
        public async Task<DbUserDetailDTO> GetUserDetailById(int id)
        {

            var userData = await _skillsheetContext.UserDetails.FirstOrDefaultAsync(u => u.Userid == id);
            if (userData == null) throw new Exception(ErrorResource.UserDetailNotExistError);
            return _mapper.Map<DbUserDetailDTO>(userData);
        }
        #endregion

       

        #region EditUserDetailAsync
        /// <summary>
        /// Edits an existing user detail asynchronously.
        /// </summary>
        /// <param name="userdata">The user detail DTO.</param>
        /// <param name="imageURL">The image URL.</param>
        /// <returns>The edited user detail model.</returns>
        public async Task<DbUserDetailDTO> EditUserDetail(UserDetailDTO userdata, string imageURL)
        {
            try
            {
                var existingUserDetail = await _skillsheetContext.UserDetails.FirstOrDefaultAsync(u => u.Username == userdata.Username);
                if (existingUserDetail == null)
                {
                    throw new Exception(ErrorResource.UserDetailNotExistError);
                }

                if (existingUserDetail != null && userdata != null)
                {
                    existingUserDetail.FullName = userdata.FullName;
                    existingUserDetail.Sex = userdata.Sex;
                    existingUserDetail.PhoneNo = userdata.PhoneNo;
                    existingUserDetail.BirthDate = userdata.Birthdate;
                    existingUserDetail.JoiningDate = userdata.JoiningDate;
                    existingUserDetail.WorkJapan = userdata.WorkJapan;
                    existingUserDetail.Qualification = userdata.Qualification ?? string.Empty;
                    existingUserDetail.Country = userdata.Country ?? string.Empty;
                    existingUserDetail.Description = userdata.Description;

                    if (!string.IsNullOrEmpty(imageURL))
                    {
                        existingUserDetail.Photo = imageURL;
                    }
                    _skillsheetContext.UserDetails.Update(existingUserDetail);
                    await _skillsheetContext.SaveChangesAsync();
                }

                return _mapper.Map<DbUserDetailDTO>(existingUserDetail);
            }
            catch (DbUpdateException)
            {
                throw new Exception(ErrorResource.DbUpdateDetailError);

            }
        }
        #endregion

        #region DeleteUserDetailAsync
        /// <summary>
        /// Deletes a user detail by username asynchronously.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>A boolean indicating success.</returns>
        public async Task<bool> DeleteUserDetail(string username)
        {
            try
            {
                var userEntity = await _skillsheetContext.UserDetails.FirstOrDefaultAsync(u => u.Username == username);
                if (userEntity == null) throw new Exception(ErrorResource.UserNotExistErrror);

                _skillsheetContext.UserDetails.Remove(userEntity);
                await _skillsheetContext.SaveChangesAsync();
                return true;
            }
            catch (DbException)
            {
                throw new Exception(ErrorResource.DbDeleteDetailError);

            }
        }
        #endregion

    }
}
