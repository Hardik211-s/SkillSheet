
using SkillSheetAPI.Models.DTOs;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserDetailRepo
    {
        Task<bool> DeleteUserDetail(string username);
        Task<List<DbUserDetailDTO>> GetAllUserDetail();
        Task<DbUserDetailDTO> GetUserDetailById(int id);
        Task<DbUserDetailDTO> EditUserDetail(UserDetailDTO userdata, string imageURL);

    }
}
