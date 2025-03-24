using AutoMapper;
using DataAccess.Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Models.DTOs;

namespace SkillSheetAPI.MapperProfiles
{
    public class UserSkillMappingProfile : Profile
    {
        public UserSkillMappingProfile()
        {
            CreateMap<UserSkill,UserSkillDTO>();
            CreateMap<UserSkillDTO,UserSkill>();
            CreateMap<DbUserSkillDTO,UserSkill>();
            CreateMap<UserAllDataDTO,UserSkill>();
            CreateMap<DbUserSkillDTO,UserSkillDTO>();
            CreateMap<UserSkill,DbUserSkillDTO>();
        }
    }
}
