
namespace SkillSheetAPI.Models.DTOs
{
    public class ChangePasswordDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string NewPassword { get; set; }
        public string Role { get; set; }
    }
}
