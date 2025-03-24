﻿using System;
using System.Collections.Generic;

namespace DataAccess.Entities.Entities;

public partial class Skill
{
    public int SkillId { get; set; }

    public int SubcategoryId { get; set; }

    public string SkillName { get; set; } = null!;

    public string? IconName { get; set; }

    public virtual SkillSubcategory Subcategory { get; set; } = null!;

    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
}
