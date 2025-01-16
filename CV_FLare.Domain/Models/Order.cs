using System;
using System.Collections.Generic;

namespace CV_FLare.Domain.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int PackageId { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? PaymentStatus { get; set; }

    public string? OrderStatus { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual Package Package { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
