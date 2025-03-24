using AutoMapper;
using DataAccess.Entities.Entities;
using SkillSheetAPI.Models.DTOs; 

namespace SkillSheetAPI.MapperProfiles
{
    public class SkillDataMappingProfile : Profile
    {
        public SkillDataMappingProfile()
        {
            CreateMap<Skill, SkillDTO>();
            CreateMap<SkillCategory, SkillCategoryDTO>();
            CreateMap<SkillSubcategory, SkillSubcategoryDTO>();


        }

    }
}
