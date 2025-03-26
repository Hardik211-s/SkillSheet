using AutoMapper;
using DataAccess.Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Models.DTOs;

namespace SkillSheetAPI.MapperProfiles
{
    public class UserDetailMappingProfile : Profile
    {
        public UserDetailMappingProfile()
        {
            CreateMap<UserDetail, UserDetailDTO>();
            CreateMap<DbUserDetailDTO, UserDetailDTO>();
            CreateMap<UserDetail, DbUserDetailDTO>();
            CreateMap< UserDetailDTO,UserDetail>();
        }
    }

}
