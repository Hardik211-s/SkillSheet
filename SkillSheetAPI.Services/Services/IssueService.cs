using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Services.Resource;

namespace SkillSheetAPI.Services.Services
{
    public class IssueService : IIssueService
    {
        private readonly IEmailService _emailService;

        public IssueService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<bool> CreateIssueAsync(CreateIssueDTO dto)
        {
            try
            {
                var msg = $@"<h2>New Issue: {dto.Title}</h2>
<p><strong>Description:</strong></p>
<p>{dto.Description}</p>
<p><strong>Reporter:</strong> {dto.ReporterName} ({dto.ReporterEmail})</p>";

                // Send to configured source/admin mail
                await _emailService.SendEmail(dto.ReporterName ?? "Reporter", string.Empty, GeneralResource.SourceMail, msg);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
