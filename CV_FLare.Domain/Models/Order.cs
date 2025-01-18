using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CV_FLare.Domain.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required]
    public int PackageId { get; set; }

    [ForeignKey("PackageId")]
    public virtual Package Package { get; set; } = null!;

    public string? PaymentMethod { get; set; }

    public DateTime OrderDate { get; set; }

    public string PaymentStatus { get; set; }

    public string OrderStatus { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalPrice { get; set; }
}
