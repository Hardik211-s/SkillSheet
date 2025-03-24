using System;
using System.Collections.Generic;

namespace DataAccess.Entities.Entities;

public partial class User
{
    public int Userid { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Username { get; set; }

    public virtual UserDetail? UserDetail { get; set; }

    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
}
