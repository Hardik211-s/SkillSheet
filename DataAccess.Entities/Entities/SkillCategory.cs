using System;
using System.Collections.Generic;

namespace DataAccess.Entities.Entities;

public partial class SkillCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? IconName { get; set; }

    public virtual ICollection<SkillSubcategory> SkillSubcategories { get; set; } = new List<SkillSubcategory>();
}
