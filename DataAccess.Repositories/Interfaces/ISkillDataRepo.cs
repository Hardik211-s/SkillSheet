
using SkillSheetAPI.Models.DTOs;

namespace DataAccess.Repositories.Interfaces
{
    public interface ISkillDataRepo
    {
        public Task<List<SkillCategoryDTO>> GetSkillCategory();
        public Task<List<SkillSubcategoryDTO>> GetSkillSubcategory();
        public Task<List<SkillDTO>> GetSkill();
        public Task<List<SkillDTO>> GetSkill(int subCategoryID);
        public Task<List<SkillSubcategoryDTO>> GetSkillSubcategory(int categoryID);

    }
}
