using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CV_FLare.Domain.Models
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [Required, Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; } = 0.00m;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}