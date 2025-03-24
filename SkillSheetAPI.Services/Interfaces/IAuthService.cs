 
using SkillSheetAPI.Models.DTOs; 

namespace SkillSheetAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<List<UserDTO>> GetAllUserService();
         Task<UserDTO> RegisterUserService(UserRegisterDTO userDto);
        Task<(UserDTO,string)> LoginUserService(UserLoginDTO userDto);
        Task<bool> UpdateUserService(UserRegisterDTO userDto);
        Task<bool> ChangePasswordService(ChangePasswordDTO changePasswordDTO);
        Task<bool> DeleteUserService(string username);
    }
}
