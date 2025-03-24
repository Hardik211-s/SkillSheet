using System;
using System.Collections.Generic;
using DataAccess.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<SkillCategory> SkillCategories { get; set; }

    public virtual DbSet<SkillSubcategory> SkillSubcategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<UserSkill> UserSkills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=skillsheet;Username=postgres;Password=Hardik@211;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("skill_primary");

            entity.ToTable("skill");

            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.IconName)
                .HasColumnType("character varying")
                .HasColumnName("icon_name");
            entity.Property(e => e.SkillName)
                .HasColumnType("character varying")
                .HasColumnName("skill_name");
            entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Skills)
                .HasForeignKey(d => d.SubcategoryId)
                .HasConstraintName("skill_foreign");
        });

        modelBuilder.Entity<SkillCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("p_key");

            entity.ToTable("skill_category");

            entity.Property(e => e.CategoryId)
                .HasDefaultValueSql("nextval('\"Skill_skillID_seq\"'::regclass)")
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasColumnType("character varying")
                .HasColumnName("category_name");
            entity.Property(e => e.IconName)
                .HasColumnType("character varying")
                .HasColumnName("icon_name");
        });

        modelBuilder.Entity<SkillSubcategory>(entity =>
        {
            entity.HasKey(e => e.SubcategoryId).HasName("category_primary");

            entity.ToTable("skill_subcategory");

            entity.HasIndex(e => e.SubcategoryId, "u_key").IsUnique();

            entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.IconName)
                .HasColumnType("character varying")
                .HasColumnName("icon_name");
            entity.Property(e => e.SubcategoryName)
                .HasMaxLength(255)
                .HasColumnName("subcategory_name");

            entity.HasOne(d => d.Category).WithMany(p => p.SkillSubcategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("f_key");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("user_pk");

            entity.ToTable("user");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasColumnType("character varying")
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("userid");

            entity.ToTable("user_detail");

            entity.HasIndex(e => e.Username, "unique_username").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("userid");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Country)
                .HasColumnType("character varying")
                .HasColumnName("country");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.FullName)
                .HasColumnType("character varying")
                .HasColumnName("full_name");
            entity.Property(e => e.JoiningDate).HasColumnName("joining_date");
            entity.Property(e => e.PhoneNo).HasColumnName("phone_no");
            entity.Property(e => e.Photo)
                .HasColumnType("character varying")
                .HasColumnName("photo");
            entity.Property(e => e.Qualification)
                .HasColumnType("character varying")
                .HasColumnName("qualification");
            entity.Property(e => e.Sex)
                .HasMaxLength(50)
                .HasColumnName("sex");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
            entity.Property(e => e.WorkJapan).HasColumnName("work_japan");

            entity.HasOne(d => d.User).WithOne(p => p.UserDetail)
                .HasForeignKey<UserDetail>(d => d.Userid)
                .HasConstraintName("user_detail_foreign");
        });

        modelBuilder.Entity<UserSkill>(entity =>
        {
            entity.HasKey(e => e.UserskillId).HasName("user_skill_pkey");

            entity.ToTable("user_skill");

            entity.Property(e => e.UserskillId).HasColumnName("userskill_id");
            entity.Property(e => e.Experience).HasColumnName("experience");
            entity.Property(e => e.ProficiencyLevel)
                .HasColumnType("character varying")
                .HasColumnName("proficiency_level");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Skill).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("user_skill_foreign");

            entity.HasOne(d => d.User).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_skill_foreign2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
