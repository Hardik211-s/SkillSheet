

namespace SkillSheetAPI.Models.DTOs
{
    public class DbUserSkillDTO
    {
        public long UserId { get; set; }

        public long SkillId { get; set; }

        public long UserskillId { get; set; }

        public string ProficiencyLevel { get; set; }

        public long Experience { get; set; }
    }
}
