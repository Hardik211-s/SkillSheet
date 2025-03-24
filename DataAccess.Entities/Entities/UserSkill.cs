using System;
using System.Collections.Generic;

namespace DataAccess.Entities.Entities;

public partial class UserSkill
{
    public int UserId { get; set; }

    public int SkillId { get; set; }

    public int UserskillId { get; set; }

    public string ProficiencyLevel { get; set; } = null!;

    public double Experience { get; set; }

    public virtual Skill Skill { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
