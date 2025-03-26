using AutoMapper;
using DataAccess.Repositories.Interfaces;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;

namespace SkillSheetAPI.Services.Services
{
    public class UserSkillService : IUserSkillService
    {

        IUserSkillRepo _skillRepo;
        private readonly IMapper _mapper;

        public UserSkillService(IUserSkillRepo userSkillRepo, IMapper mapper)
        {
            this._skillRepo = userSkillRepo;
            this._mapper = mapper;
        }
        #region AllUserSkillService
        /// <summary>
        /// Gets all user skills asynchronously.
        /// </summary>
        /// <returns>A list of all user skills.</returns>
        public async Task<List<UserAllDataDTO>> AllUserSkillService()
        {
            try
            {
                var userSkill = await _skillRepo.AllUserSkill();
                return _mapper.Map<List<UserAllDataDTO>>(userSkill);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetUserSkillService
        /// <summary>
        /// Gets user skills by user ID asynchronously.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns>A list of user skills.</returns>
        public async Task<List<UserAllDataDTO>> GetUserSkillService(int userID)
        {
            try
            {
                var userSkill = await _skillRepo.GetUserSkill(userID);
                return _mapper.Map<List<UserAllDataDTO>>(userSkill);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region AddUserSkillService
        /// <summary>
        /// Adds a new user skill asynchronously.
        /// </summary>
        /// <param name="userSkillDTO">The user skill DTO.</param>
        /// <returns>The added user skill.</returns>
        public async Task<DbUserSkillDTO> AddUserSkillService(UserSkillDTO userSkillDTO)
        {
            try
            {
                var userSkill = await _skillRepo.AddUserSkill(userSkillDTO);
                return userSkill;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region EditUserSkillService
        /// <summary>
        /// Edits an existing user skill asynchronously.
        /// </summary>
        /// <param name="userSkillDTO">The user skill DTO.</param>
        /// <returns>The edited user skill.</returns>
        public async Task<DbUserSkillDTO> EditUserSkillService(DbUserSkillDTO userSkillDTO)
        {
            try
            {
                var userSkill = await _skillRepo.EditUserSkill(userSkillDTO);
                return _mapper.Map<DbUserSkillDTO>(userSkill);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region DeleteUserSkillService
        /// <summary>
        /// Deletes a user skill by ID asynchronously.
        /// </summary>
        /// <param name="userSkillID">The user skill ID.</param>
        /// <returns>The deleted user skill.</returns>
        public async Task<DbUserSkillDTO> DeleteUserSkillService(int userSkillID)
        {
            try
            {
                var userSkill = await _skillRepo.DeleteUserSkill(userSkillID);
                return _mapper.Map<DbUserSkillDTO>(userSkill);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion
    }
}
