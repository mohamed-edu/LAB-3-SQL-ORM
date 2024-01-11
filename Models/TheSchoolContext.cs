using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LAB_3_SQL_ORM.Models;

public partial class TheSchoolContext : DbContext
{
    public TheSchoolContext()
    {
    }

    public TheSchoolContext(DbContextOptions<TheSchoolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=TheSchool;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("CourseID");
            entity.Property(e => e.CourseName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.FkCourseId).HasColumnName("FK_CourseID");
            entity.Property(e => e.FkStudentId).HasColumnName("FK_StudentID");
            entity.Property(e => e.FkTeacherId).HasColumnName("FK_TeacherID");
            entity.Property(e => e.Grade1).HasColumnName("Grade");
            entity.Property(e => e.GradeId).HasColumnName("GradeID");

            entity.HasOne(d => d.FkCourse).WithMany()
                .HasForeignKey(d => d.FkCourseId)
                .HasConstraintName("FK_Grades_Courses");

            entity.HasOne(d => d.FkStudent).WithMany()
                .HasForeignKey(d => d.FkStudentId)
                .HasConstraintName("FK_Grades_Student");

            entity.HasOne(d => d.FkTeacher).WithMany()
                .HasForeignKey(d => d.FkTeacherId)
                .HasConstraintName("FK_Grades_Teacher");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.PersonId)
                .ValueGeneratedNever()
                .HasColumnName("PersonID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.StudentId)
                .ValueGeneratedNever()
                .HasColumnName("StudentID");
            entity.Property(e => e.Class)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FkPersonId).HasColumnName("FK_PersonID");

            entity.HasOne(d => d.FkPerson).WithMany(p => p.Students)
                .HasForeignKey(d => d.FkPersonId)
                .HasConstraintName("FK_Student_People");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teacher");

            entity.Property(e => e.TeacherId)
                .ValueGeneratedNever()
                .HasColumnName("TeacherID");
            entity.Property(e => e.FkPersonId).HasColumnName("FK_personID");
            entity.Property(e => e.SubjectField)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.FkPerson).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.FkPersonId)
                .HasConstraintName("FK_Teacher_People");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
