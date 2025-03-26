using System;
using System.Collections.Generic;

namespace DataAccess.Entities.Entities;

public partial class SkillSubcategory
{
    public int SkillSubcategoryId { get; set; }

    public int SkillCategoryId { get; set; }

    public string SkillSubcategoryName { get; set; } = null!;

    public string? IconName { get; set; }

    public virtual SkillCategory SkillCategory { get; set; } = null!;

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
