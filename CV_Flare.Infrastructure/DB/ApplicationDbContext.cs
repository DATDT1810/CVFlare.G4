using CV_FLare.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Infrastructure.DB
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=TANDAT\\MSSQLSERVER03;Database=CVFlareG4;uid=sa;pwd=1234;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True");
        }



        public DbSet<CvEdit> CvEdits { get; set; }

        public DbSet<CvSubmission> CvSubmissions { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Package> Packages { get; set; }

        public DbSet<Template> Templates { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserTemplate> UserTemplates { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vô hiệu hóa Cascade Delete cho SubmissionId
            modelBuilder.Entity<CvEdit>()
                .HasOne(e => e.Submission)
                .WithMany(s => s.CvEdits)
                .HasForeignKey(e => e.SubmissionId)
                .OnDelete(DeleteBehavior.Restrict); // Sử dụng Restrict hoặc SetNull thay vì Cascade

            // Vô hiệu hóa Cascade Delete cho UserId
            modelBuilder.Entity<CvEdit>()
                .HasOne(e => e.EditedByNavigation)
                .WithMany(u => u.CvEdits)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Sử dụng Restrict hoặc SetNull thay vì Cascade
        }

    }
}
