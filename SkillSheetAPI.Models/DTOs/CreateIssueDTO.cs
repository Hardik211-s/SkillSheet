namespace SkillSheetAPI.Models.DTOs
{
    public class CreateIssueDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReporterName { get; set; }
        public string ReporterEmail { get; set; }
    }
}
