using System;

using SkillSheetAPI.Models.DTOs;

namespace DataAccess.Repositories.Interfaces
{
    public interface IAuthRepo
    {
        Task<List<UserDTO>> GetAllUser();
        Task<UserDTO> GetUserByUsername(string username);
        Task<UserDTO> LoginUser(UserLoginDTO userLoginDTO);
        Task<UserDTO> RegisterUser(UserRegisterDTO userRegisterDTO);
        Task<bool> UpdateUser(UserRegisterDTO userRegisterDTO);
        Task<bool> CheckUserPassword(UserRegisterDTO userData);
        Task<bool> DeleteUser(string id);

        string GenerateJwtToken(UserDTO user);

    }
}
