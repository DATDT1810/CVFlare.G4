namespace CV_Flare.RazorPage.ViewModels
{
    public class TemplatesVM
    {
        public int TemplateId { get; set; }

        public string? TemplateName { get; set; }

        public string? PreviewImg { get; set; }

        public string? TemplateFile { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}
