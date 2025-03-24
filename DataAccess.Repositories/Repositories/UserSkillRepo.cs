
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
            var existingUserSkill = await _skillsheetContext.UserSkills.Select(o => new UserAllDataDTO
            {
                Username = o.User.Username ?? string.Empty,
                Skill = o.Skill.SkillName,
                Subcategory = o.Skill.Subcategory.SubcategoryName,
                Category = o.Skill.Subcategory.Category.CategoryName,
                ProficiencyLevel = o.ProficiencyLevel,
                Experience = o.Experience,
                IconName = o.Skill.IconName ?? string.Empty,
                UserSkillId = o.UserskillId
            })
                .ToListAsync();

            if (existingUserSkill == null) throw new Exception(ErrorResource.SkillNotAvailableError);

            return existingUserSkill;
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
            var existingUserSkill = await _skillsheetContext.UserSkills
                .Where(user => user.UserId == userID)
                .Select(userdata => new UserAllDataDTO
                {
                    Username = userdata.User.Username ?? string.Empty,
                    Skill = userdata.Skill.SkillName,
                    Subcategory = userdata.Skill.Subcategory.SubcategoryName,
                    Category = userdata.Skill.Subcategory.Category.CategoryName,
                    ProficiencyLevel = userdata.ProficiencyLevel,
                    Experience = userdata.Experience,
                    IconName = userdata.Skill.IconName ?? string.Empty,
                    UserSkillId = userdata.UserskillId
                })
                .ToListAsync();

            if (existingUserSkill == null) throw new Exception(ErrorResource.SkillNotAvailableError);

            return existingUserSkill;
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
            int size = userSkillDTO.MyId.Length;
            var userSkillDetail = new UserSkill();

            while (size != 0)
            {
                bool exists = await _skillsheetContext.UserSkills
                    .AnyAsync(us => us.SkillId == userSkillDTO.MyId[size - 1] && us.UserId == userSkillDTO.UserId);
                var userSkill = _mapper.Map<UserSkill>(userSkillDTO);
                if (exists)
                {
                    var skillPresence = await _skillsheetContext.UserSkills.FirstOrDefaultAsync(o => o.UserId == userSkillDTO.UserId && o.SkillId == userSkillDTO.MyId[size - 1]);
                    if (skillPresence != null)
                    {
                        skillPresence.ProficiencyLevel = userSkillDTO.ProficiencyLevel;
                        skillPresence.Experience = userSkillDTO.Experience;
                    }

                    var userSkillDetail1 = _skillsheetContext.UserSkills.Update(skillPresence!).Entity;
                    await _skillsheetContext.SaveChangesAsync();

                    size -= 1;
                    continue;
                }

                userSkill.SkillId = userSkillDTO.MyId[size - 1];
                userSkillDetail = _skillsheetContext.UserSkills.Add(userSkill).Entity;
                await _skillsheetContext.SaveChangesAsync();
                size -= 1;
            }

            return _mapper.Map<DbUserSkillDTO>(userSkillDetail);
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
        #endregion

        #region DeleteUserSkillAsync
        /// <summary>
        /// Deletes a user skill by ID asynchronously.
        /// </summary>
        /// <param name="userSkillID">The user skill ID.</param>
        /// <returns>The deleted user skill.</returns>
        public async Task<DbUserSkillDTO> DeleteUserSkill(int userSkillID)
        {
            var existingUserSkill = await _skillsheetContext.UserSkills
                .FirstOrDefaultAsync(u => u.UserskillId == userSkillID);
            if (existingUserSkill == null) throw new Exception(ErrorResource.SkillNotAvailableError);

            var userSkillDetail = _skillsheetContext.UserSkills.Remove(existingUserSkill).Entity;
            await _skillsheetContext.SaveChangesAsync();

            return _mapper.Map<DbUserSkillDTO>(userSkillDetail);
        }
        #endregion
    }
}