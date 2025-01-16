using System;
using System.Collections.Generic;

namespace CV_FLare.Domain.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserFullname { get; set; }

    public string? UserEmail { get; set; }

    public string? UserPassword { get; set; }

    public int? UserRole { get; set; }

    public string? UserPhone { get; set; }

    public DateTime? UserCreateAt { get; set; }

    public DateTime? UserUpdateAt { get; set; }

    public string? UserImg { get; set; }

    public virtual ICollection<CvEdit> CvEdits { get; set; } = new List<CvEdit>();

    public virtual ICollection<CvSubmission> CvSubmissions { get; set; } = new List<CvSubmission>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ServiceRating> ServiceRatings { get; set; } = new List<ServiceRating>();

    public virtual ICollection<UserTemplate> UserTemplates { get; set; } = new List<UserTemplate>();
}
