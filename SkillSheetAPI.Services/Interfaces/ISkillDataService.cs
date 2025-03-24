
using SkillSheetAPI.Models.DTOs;

namespace SkillSheetAPI.Services.Interfaces
{
    public interface ISkillDataService
    {
        public Task<List<SkillCategoryDTO>> GetSkillCategoryService();
        public Task<List<SkillSubcategoryDTO>> GetSkillSubcategoryService();
        public Task<List<SkillDTO>> GetSkillService();
        public Task<List<SkillDTO>> GetSkillService(int subCategoryID);
        public Task<List<SkillSubcategoryDTO>> GetSkillSubcategoryService(int categoryID);


    }
}
