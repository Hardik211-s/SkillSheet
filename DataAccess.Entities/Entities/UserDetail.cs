using System;
using System.Collections.Generic;

namespace DataAccess.Entities.Entities;

public partial class UserDetail
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public DateOnly JoiningDate { get; set; }

    public bool WorkJapan { get; set; }

    public string Photo { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Qualification { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public long PhoneNo { get; set; }

    public virtual User User { get; set; } = null!;
}
