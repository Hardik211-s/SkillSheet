
using System.Data.Common;
using AutoMapper;
using DataAccess.Entities.Context;
using DataAccess.Entities.Entities;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories.Resource;
using Microsoft.EntityFrameworkCore;
using SkillSheetAPI.Models.DTOs;

namespace DataAccess.Repositories.Repositories
{
    public class UserSkillRepo : IUserSkillRepo
    {
        ApplicationDbContext _skillsheetContext;
        private readonly IMapper _mapper;

        public UserSkillRepo(ApplicationDbContext skillsheetContext, IMapper mapper)
        {
            _skillsheetContext = skillsheetContext;
            _mapper = mapper;
        }


        #region AllUserSkillAsync
        /// <summary>
        /// Gets all user skills asynchronously.
        /// </summary>
        /// <returns>A list of all user skills.</returns>
        public async Task<List<UserAllDataDTO>> AllUserSkill()
        {
            var allUserSkills = _skillsheetContext.UserSkills;
                
            if (allUserSkills == null) throw new Exception(ErrorResource.SkillNotAvailableError);
               
                
            var allUserSkillData=await allUserSkills.Select(o => new UserAllDataDTO
            {
                Username = o.User.Username ?? string.Empty,
                Skill = o.Skill.SkillName,
                Subcategory = o.Skill.SkillSubcategory.SkillSubcategoryName,
                Category = o.Skill.SkillSubcategory.SkillCategory.SkillCategoryName,
                ProficiencyLevel = o.ProficiencyLevel,
                Experience = o.Experience,
                IconName = o.Skill.IconName ?? string.Empty,
                UserSkillId = o.UserskillId
            }).ToListAsync();


            return allUserSkillData;
        }
        #endregion

        #region GetUserSkillAsync
        /// <summary>
        /// Gets user skills by user ID asynchronously.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns>A list of user skills.</returns>
        public async Task<List<UserAllDataDTO>> GetUserSkill(int userID)
        {
            var allUserSkills =  _skillsheetContext.UserSkills;

            if (allUserSkills == null) throw new Exception(ErrorResource.SkillNotAvailableError);

            var userSkill= await allUserSkills.Where(user => user.UserId == userID)
                .Select(userdata => new UserAllDataDTO
                {
                    Username = userdata.User.Username ?? string.Empty,
                    Skill = userdata.Skill.SkillName,
                    Subcategory = userdata.Skill.SkillSubcategory.SkillSubcategoryName,
                    Category = userdata.Skill.SkillSubcategory.SkillCategory.SkillCategoryName,
                    ProficiencyLevel = userdata.ProficiencyLevel,
                    Experience = userdata.Experience,
                    IconName = userdata.Skill.IconName ?? string.Empty,
                    UserSkillId = userdata.UserskillId
                })
                .ToListAsync();

            if (userSkill.Count == 0) throw new Exception(ErrorResource.SkillNotAvailableError);

            return userSkill;
        }
        #endregion

        #region AddUserSkillAsync
        /// <summary>
        /// Adds a new user skill asynchronously.
        /// </summary>
        /// <param name="userSkillDTO">The user skill DTO.</param>
        /// <returns>The added user skill.</returns>
        public async Task<DbUserSkillDTO> AddUserSkill(UserSkillDTO userSkillDTO)
        {
            try
            {
                var userSkillDetail = new UserSkill();

                
              
                bool exists = await _skillsheetContext.UserSkills
                        .AnyAsync(us => us.SkillId == userSkillDTO.MyId && us.UserId == userSkillDTO.UserId);
                var userSkill = _mapper.Map<UserSkill>(userSkillDTO);
                if (exists)
                {
                    var skillPresence = await _skillsheetContext.UserSkills.FirstOrDefaultAsync(o => o.UserId == userSkillDTO.UserId && o.SkillId == userSkillDTO.MyId);
                    if (skillPresence != null)
                    {
                        skillPresence.ProficiencyLevel = userSkillDTO.ProficiencyLevel;
                        skillPresence.Experience = userSkillDTO.Experience;
                    }

                    var userSkillDetail1 = _skillsheetContext.UserSkills.Update(skillPresence!).Entity;
                    await _skillsheetContext.SaveChangesAsync();

                }
                else
                {

                    userSkill.SkillId = userSkillDTO.MyId;
                    userSkillDetail = _skillsheetContext.UserSkills.Add(userSkill).Entity;
                    await _skillsheetContext.SaveChangesAsync();
                }
                

                return _mapper.Map<DbUserSkillDTO>(userSkillDetail);
            }
            catch (DbException)
            {
                throw new Exception(ErrorResource.DbUserSkillAddError);
            }
        }
        #endregion

        #region EditUserSkillAsync
        /// <summary>
        /// Edits an existing user skill asynchronously.
        /// </summary>
        /// <param name="userSkillDTO">The user skill DTO.</param>
        /// <returns>The edited user skill.</returns>
        public async Task<DbUserSkillDTO> EditUserSkill(DbUserSkillDTO userSkillDTO)
        {
            try
            {
                var existingUserSkill = await _skillsheetContext.UserSkills
                    .FirstOrDefaultAsync(u => u.UserskillId == userSkillDTO.UserskillId);
                if (existingUserSkill == null) throw new Exception(ErrorResource.SkillNotAvailableError);

                var userSkill = _mapper.Map<UserSkill>(userSkillDTO);
                existingUserSkill.ProficiencyLevel = userSkillDTO.ProficiencyLevel;
                existingUserSkill.Experience = userSkillDTO.Experience;

                var userSkillDetail = _skillsheetContext.UserSkills.Update(existingUserSkill).Entity;
                await _skillsheetContext.SaveChangesAsync();

                return _mapper.Map<DbUserSkillDTO>(userSkillDetail);
            }
            catch (DbException)
            {
                throw new Exception(ErrorResource.DbUserSkillUpdateError);
            }
        }
        #endregion

        #region DeleteUserSkillAsync
        /// <summary>
        /// Deletes a user skill by ID asynchronously.
        /// </summary>
        /// <param name="userSkillID">The user skill ID.</param>
        /// <returns>The deleted user skill.</returns>
        public async Task<DbUserSkillDTO> DeleteUserSkill(int userSkillID)
        {
            try
            {
                var existingUserSkill = await _skillsheetContext.UserSkills
                    .FirstOrDefaultAsync(u => u.UserskillId == userSkillID);
                if (existingUserSkill == null) throw new Exception(ErrorResource.SkillNotAvailableError);

                var userSkillDetail = _skillsheetContext.UserSkills.Remove(existingUserSkill).Entity;
                await _skillsheetContext.SaveChangesAsync();

                return _mapper.Map<DbUserSkillDTO>(userSkillDetail);
            }
            catch (DbException)
            {
                throw new Exception(ErrorResource.DbUserSkillDeleteError);

            }
        }
        #endregion
    }
}