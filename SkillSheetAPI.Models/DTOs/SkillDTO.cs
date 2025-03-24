

namespace SkillSheetAPI.Models.DTOs
{
    public class SkillDTO
    {
        public long SkillId { get; set; }

        public long SubcategoryId { get; set; }
        public string? IconName { get; set; }

        public string SkillName { get; set; } 
    }
}
