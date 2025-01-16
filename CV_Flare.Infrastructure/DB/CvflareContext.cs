using System;
using System.Collections.Generic;
using CV_FLare.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CV_FLare.Infrastructure.DB;

public partial class CvflareContext : DbContext
{
    public CvflareContext()
    {
    }

    public CvflareContext(DbContextOptions<CvflareContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CvEdit> CvEdits { get; set; }

    public virtual DbSet<CvSubmission> CvSubmissions { get; set; }

    public virtual DbSet<JobDescription> JobDescriptions { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<ServiceRating> ServiceRatings { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTemplate> UserTemplates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=TANDAT\\MSSQLSERVER03;Database=CVFlare;uid=sa;pwd=1234;Trusted_Connection=False;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CvEdit>(entity =>
        {
            entity.HasKey(e => e.EditId).HasName("PK__CV_Edits__A8C1B4CC97123CA9");

            entity.ToTable("CV_Edits");

            entity.Property(e => e.EditId).HasColumnName("edit_id");
            entity.Property(e => e.EditedAt)
                .HasColumnType("datetime")
                .HasColumnName("edited_at");
            entity.Property(e => e.EditedBy).HasColumnName("edited_by");
            entity.Property(e => e.EditedContent).HasColumnName("edited_content");
            entity.Property(e => e.Feedback).HasColumnName("feedback");
            entity.Property(e => e.SubmissionId).HasColumnName("submission_id");

            entity.HasOne(d => d.EditedByNavigation).WithMany(p => p.CvEdits)
                .HasForeignKey(d => d.EditedBy)
                .HasConstraintName("FK_CV_Edits_Users");

            entity.HasOne(d => d.Submission).WithMany(p => p.CvEdits)
                .HasForeignKey(d => d.SubmissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CV_Edits_CV_Submissions");
        });

        modelBuilder.Entity<CvSubmission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId).HasName("PK__CV_Submi__9B535595804C26DC");

            entity.ToTable("CV_Submissions");

            entity.Property(e => e.SubmissionId).HasColumnName("submission_id");
            entity.Property(e => e.AiScore).HasColumnName("ai_score");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("due_date");
            entity.Property(e => e.FilePath).HasColumnName("file_path");
            entity.Property(e => e.JobDescId).HasColumnName("job_desc_id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UploadedAt)
                .HasColumnType("datetime")
                .HasColumnName("uploaded_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.JobDesc).WithMany(p => p.CvSubmissions)
                .HasForeignKey(d => d.JobDescId)
                .HasConstraintName("FK_CV_Submissions_Job_Descriptions");

            entity.HasOne(d => d.Package).WithMany(p => p.CvSubmissions)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CV_Submissions_Packages");

            entity.HasOne(d => d.User).WithMany(p => p.CvSubmissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_CV_Submissions_Users");
        });

        modelBuilder.Entity<JobDescription>(entity =>
        {
            entity.HasKey(e => e.JobDescId).HasName("PK__Job_Desc__01FEFB77DC2DC6D9");

            entity.ToTable("Job_Descriptions");

            entity.Property(e => e.JobDescId).HasColumnName("job_desc_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.JobDescription1).HasColumnName("job_description");
            entity.Property(e => e.JobTitle)
                .HasMaxLength(255)
                .HasColumnName("job_title");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__46596229F16E53A8");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .HasColumnName("order_status");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("payment_status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Package).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Packages");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__Packages__63846AE8B3670D5C");

            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.PackageDescription).HasColumnName("package_description");
            entity.Property(e => e.PackageName)
                .HasMaxLength(255)
                .HasColumnName("package_name");
            entity.Property(e => e.PackagePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("package_price");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<ServiceRating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Service___D35B278BAD01C0B9");

            entity.ToTable("Service_Ratings");

            entity.Property(e => e.RatingId).HasColumnName("rating_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.RatingIcon).HasColumnName("rating_icon");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceRatings)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Service_Ratings_Packages");

            entity.HasOne(d => d.User).WithMany(p => p.ServiceRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Service_Ratings_Users");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.TemplateId).HasName("PK__Template__BE44E07962FB4294");

            entity.Property(e => e.TemplateId).HasColumnName("template_id");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.PreviewImg).HasColumnName("preview_img");
            entity.Property(e => e.TemplateFile).HasColumnName("template_file");
            entity.Property(e => e.TemplateName)
                .HasMaxLength(255)
                .HasColumnName("template_name");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("update_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FCA452488");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserCreateAt)
                .HasColumnType("datetime")
                .HasColumnName("user_createAt");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .HasColumnName("user_email");
            entity.Property(e => e.UserFullname)
                .HasMaxLength(255)
                .HasColumnName("user_fullname");
            entity.Property(e => e.UserImg).HasColumnName("user_img");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .HasColumnName("user_password");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(50)
                .HasColumnName("user_phone");
            entity.Property(e => e.UserRole).HasColumnName("user_role");
            entity.Property(e => e.UserUpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("user_updateAt");
        });

        modelBuilder.Entity<UserTemplate>(entity =>
        {
            entity.HasKey(e => e.UserTemplatesId).HasName("PK__User_Tem__3F070B074B63D9DD");

            entity.ToTable("User_Templates");

            entity.Property(e => e.UserTemplatesId).HasColumnName("user_templates_id");
            entity.Property(e => e.AiScore).HasColumnName("ai_score");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.CvName)
                .HasMaxLength(255)
                .HasColumnName("cv_name");
            entity.Property(e => e.IsDraft).HasColumnName("is_draft");
            entity.Property(e => e.JobDescId).HasColumnName("job_desc_id");
            entity.Property(e => e.TemplateContent).HasColumnName("template_content");
            entity.Property(e => e.TemplateId).HasColumnName("template_id");
            entity.Property(e => e.TemplateProgress).HasColumnName("template_progress");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("update_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.JobDesc).WithMany(p => p.UserTemplates)
                .HasForeignKey(d => d.JobDescId)
                .HasConstraintName("FK_User_Templates_Job_Descriptions");

            entity.HasOne(d => d.Template).WithMany(p => p.UserTemplates)
                .HasForeignKey(d => d.TemplateId)
                .HasConstraintName("FK_User_Templates_Templates");

            entity.HasOne(d => d.User).WithMany(p => p.UserTemplates)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_Templates_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
