
using Microsoft.AspNetCore.Http;

namespace SkillSheetAPI.Models.DTOs
{
    public class UserDetailDTO
    {

        public string Username { get; set; } = null!;

        public string Sex { get; set; } = null!;

        public DateOnly Birthdate { get; set; }

        public DateOnly JoiningDate { get; set; }

        public bool WorkJapan { get; set; }

        public IFormFile? Photo { get; set; }
        public string? Qualification { get; set; }
        public string? Country { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public long PhoneNo { get; set; }

    }
}
