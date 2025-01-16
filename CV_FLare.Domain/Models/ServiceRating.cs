using System;
using System.Collections.Generic;

namespace CV_FLare.Domain.Models;

public partial class ServiceRating
{
    public int RatingId { get; set; }

    public int UserId { get; set; }

    public int ServiceId { get; set; }

    public int? RatingIcon { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Package Service { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
