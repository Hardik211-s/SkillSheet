

namespace SkillSheetAPI.Services.Interfaces
{
    public interface IEmailService
    {
        public  Task SendEmail(string username,string password,string email,string msg);
    }
}
