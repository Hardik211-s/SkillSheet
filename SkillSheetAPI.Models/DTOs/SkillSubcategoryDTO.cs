
namespace SkillSheetAPI.Models.DTOs
{
    public class SkillSubcategoryDTO
    {
        public long SkillSubcategoryId { get; set; }

        public long SkillCategoryId { get; set; }

        public string SkillSubcategoryName { get; set; }
        public string? IconName { get; set; }

    }
}
