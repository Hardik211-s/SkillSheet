
using SkillSheetAPI.Models.DTOs;

namespace SkillSheetAPI.Services.Interfaces
{
    public interface IUserSkillService
    {
        public Task<List<UserAllDataDTO>> GetUserSkillService(int userID);
        public Task<DbUserSkillDTO> AddUserSkillService(UserSkillDTO userSkillDTO);
        public Task<DbUserSkillDTO> EditUserSkillService(DbUserSkillDTO userSkillDTO); 

        public Task<DbUserSkillDTO> DeleteUserSkillService(int userSkillID);
        public Task<List<UserAllDataDTO>> AllUserSkillService();

    }
}
