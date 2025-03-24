
namespace SkillSheetAPI.Models.DTOs
{
    public class UserAllDataDTO
    {

        public string Username { get; set; } = null!;
        public long UserSkillId { get; set; }
        public string Skill { get; set; } = null!;
        public string IconName { get; set; } = null!;
        public string Subcategory { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string ProficiencyLevel { get; set; } = null!;
        public double Experience { get; set; }
    }
}
