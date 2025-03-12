using CV_FLare.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace CV_Flare.RazorPage.ViewModels
{
    public class CvSubmissionVM
    {
        public int SubmissionId { get; set; }
        public int PackageId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string? FilePath { get; set; }
        public string JobDescripion { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public DateTime? UploadedAt { get; set; }
        [Required]
        public IFormFile file { get; set; }
    }
}
