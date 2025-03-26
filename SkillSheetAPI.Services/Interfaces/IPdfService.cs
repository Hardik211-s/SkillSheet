
namespace SkillSheetAPI.Services.Interfaces
{
    public interface IPdfService
    {
        public  Task<byte[]> CreatePDFAsync(string title, string content);
    }
}
