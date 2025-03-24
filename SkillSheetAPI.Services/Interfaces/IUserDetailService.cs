﻿
using SkillSheetAPI.Models.DTOs;

namespace SkillSheetAPI.Services.Interfaces
{
    public interface IUserDetailService
    {

        Task<List<DbUserDetailDTO>> GetAllUserService();
        Task<bool> AddUserDetailService(UserDetailDTO userDetailDto);
        Task<DbUserDetailDTO> GetUserDetailService(int id);
        Task<bool> DeleteUserDetailService(string username);
        Task<bool> EditUserDetailService(UserDetailDTO userDetailDto);
    }
}
