
namespace SkillSheetAPI.Models.DTOs
{
    public class SkillSubcategoryDTO
    {
        public long SubcategoryId { get; set; }

        public long CategoryId { get; set; }

        public string SubcategoryName { get; set; }
        public string? IconName { get; set; }

    }
}
