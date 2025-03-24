
using SkillSheetAPI.Models.DTOs;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserSkillRepo
    {
        public Task<List<UserAllDataDTO>> GetUserSkill(int userID);

        public Task<DbUserSkillDTO> AddUserSkill(UserSkillDTO userSkillDTO);
        public Task<DbUserSkillDTO> EditUserSkill(DbUserSkillDTO userSkillDTO);
        public Task<DbUserSkillDTO> DeleteUserSkill(int userSkillID);
        public Task<List<UserAllDataDTO>> AllUserSkill();




    }
}
