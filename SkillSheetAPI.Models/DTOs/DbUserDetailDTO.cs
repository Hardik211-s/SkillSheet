

namespace SkillSheetAPI.Models.DTOs
{
    public class DbUserDetailDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;

        public string Sex { get; set; }

        public DateOnly Birthdate { get; set; }

        public DateOnly JoiningDate { get; set; }

        public bool WorkJapan { get; set; }

        public string Photo { get; set; }
        public string? Qualification { get; set; }
        public string? Country { get; set; }
        public string Description { get; set; } 

        public string FullName { get; set; } 

        public long PhoneNo { get; set; }
        public int Age { get; set; }

    }
}
