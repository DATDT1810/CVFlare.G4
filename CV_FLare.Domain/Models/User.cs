using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_FLare.Domain.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [MaxLength(255)]
    public string? UserFullname { get; set; }

    [MaxLength(255)]
    public string? Username { get; set; }


    [Required]
    [MaxLength(255)]
    public string UserEmail { get; set; }

    [Required]
    [MaxLength(50)]
    public string UserPassword { get; set; }

    public int UserRole { get; set; }

    [Required]
    public string AspUId { get; set; }  // Đây là khóa ngoại trỏ đến bảng AspNetUsers

    [ForeignKey("AspUId")]
    public virtual IdentityUser UserIdentity { get; set; } = null!;

    [MaxLength(50)]
    public string? UserPhone { get; set; }

    public DateTime? UserCreateAt { get; set; }

    public DateTime? UserUpdateAt { get; set; }

    public string? UserImg { get; set; }

    public virtual Wallet Wallet { get; set; } = null!;

    public virtual ICollection<CvEdit> CvEdits { get; set; } = new List<CvEdit>();

    public virtual ICollection<CvSubmission> CvSubmissions { get; set; } = new List<CvSubmission>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ServiceRating> ServiceRatings { get; set; } = new List<ServiceRating>();

    public virtual ICollection<UserTemplate> UserTemplates { get; set; } = new List<UserTemplate>();
}
