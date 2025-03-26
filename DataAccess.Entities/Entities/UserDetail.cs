using System;
using System.Collections.Generic;

namespace DataAccess.Entities.Entities;

public partial class UserDetail
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string? Sex { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateOnly? JoiningDate { get; set; }

    public bool? WorkJapan { get; set; }

    public string? Photo { get; set; }

    public string? Description { get; set; }

    public string? Country { get; set; }

    public string? Qualification { get; set; }

    public string? FullName { get; set; }

    public long? PhoneNo { get; set; }

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
}
