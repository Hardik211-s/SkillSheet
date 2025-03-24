using System;
using System.Collections.Generic;

namespace DataAccess.Entities.Entities;

public partial class SkillSubcategory
{
    public int SubcategoryId { get; set; }

    public int CategoryId { get; set; }

    public string SubcategoryName { get; set; } = null!;

    public string? IconName { get; set; }

    public virtual SkillCategory Category { get; set; } = null!;

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
