
using Microsoft.AspNetCore.Http;

namespace SkillSheetAPI.Models.DTOs
{
    public class UserDetailDTO
    {

        public string Username { get; set; }

        public string Sex { get; set; }

        public DateOnly Birthdate { get; set; }

        public DateOnly JoiningDate { get; set; }

        public bool WorkJapan { get; set; }

        public IFormFile? Photo { get; set; } 
        public string? Qualification { get; set; }
        public string? Country { get; set; }
        public string Description { get; set; } 

        public string FullName { get; set; } 

        public long PhoneNo { get; set; }

    }
}
