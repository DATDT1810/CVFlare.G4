using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.DTOs
{
    public class CvSubmissionDTO
    {
        public int SubmissionId { get; set; }
        public int PackageId { get; set; }
        public int UserId { get; set; }
        [Required]
        public IFormFile file { get; set; }
        public string? FilePath { get; set; }
        public string JobDescripion { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public DateTime? UploadedAt { get; set; }
    }
}
