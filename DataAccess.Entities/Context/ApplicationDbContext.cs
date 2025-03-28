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

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<UserSkill> UserSkills { get; set; }


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
            entity.Property(e => e.SkillSubcategoryId).HasColumnName("skill_subcategory_id");

            entity.HasOne(d => d.SkillSubcategory).WithMany(p => p.Skills)
                .HasForeignKey(d => d.SkillSubcategoryId)
                .HasConstraintName("skill_foreign");
        });

        modelBuilder.Entity<SkillCategory>(entity =>
        {
            entity.HasKey(e => e.SkillCategoryId).HasName("p_key");

            entity.ToTable("skill_category");

            entity.Property(e => e.SkillCategoryId)
                .HasDefaultValueSql("nextval('\"Skill_skillID_seq\"'::regclass)")
                .HasColumnName("skill_category_id");
            entity.Property(e => e.IconName)
                .HasColumnType("character varying")
                .HasColumnName("icon_name");
            entity.Property(e => e.SkillCategoryName)
                .HasColumnType("character varying")
                .HasColumnName("skill_category_name");
        });

        modelBuilder.Entity<SkillSubcategory>(entity =>
        {
            entity.HasKey(e => e.SkillSubcategoryId).HasName("category_primary");

            entity.ToTable("skill_subcategory");

            entity.HasIndex(e => e.SkillSubcategoryId, "u_key").IsUnique();

            entity.Property(e => e.SkillSubcategoryId)
                .HasDefaultValueSql("nextval('skill_subcategory_subcategory_id_seq'::regclass)")
                .HasColumnName("skill_subcategory_id");
            entity.Property(e => e.IconName)
                .HasColumnType("character varying")
                .HasColumnName("icon_name");
            entity.Property(e => e.SkillCategoryId).HasColumnName("skill_category_id");
            entity.Property(e => e.SkillSubcategoryName)
                .HasMaxLength(255)
                .HasColumnName("skill_subcategory_name");

            entity.HasOne(d => d.SkillCategory).WithMany(p => p.SkillSubcategories)
                .HasForeignKey(d => d.SkillCategoryId)
                .HasConstraintName("f_key");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("userid");

            entity.ToTable("user_detail");

            entity.HasIndex(e => e.Username, "unique_username").IsUnique();

            entity.Property(e => e.Userid)
                .HasDefaultValueSql("nextval('\"Skill_skillID_seq\"'::regclass)")
                .HasColumnName("userid");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Country)
                .HasColumnType("character varying")
                .HasColumnName("country");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasColumnType("character varying")
                .HasColumnName("full_name");
            entity.Property(e => e.JoiningDate).HasColumnName("joining_date");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNo).HasColumnName("phone_no");
            entity.Property(e => e.Photo)
                .HasColumnType("character varying")
                .HasColumnName("photo");
            entity.Property(e => e.Qualification)
                .HasColumnType("character varying")
                .HasColumnName("qualification");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .HasColumnName("role");
            entity.Property(e => e.Sex)
                .HasMaxLength(50)
                .HasColumnName("sex");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
            entity.Property(e => e.WorkJapan).HasColumnName("work_japan");
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
                .HasConstraintName("user_skill_foreign1");

            entity.HasOne(d => d.User).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_skill_foreign2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
