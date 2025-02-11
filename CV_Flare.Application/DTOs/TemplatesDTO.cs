using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.DTOs
{
    public class TemplatesDTO
    {
        public int TemplateId { get; set; }
        public string? TemplateName { get; set; }
        public bool isDeleted { get; set; }
        public string? PreviewImg { get; set; }
        public string? TemplateFile { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
