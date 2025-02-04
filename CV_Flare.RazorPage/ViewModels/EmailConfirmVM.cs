using System.ComponentModel.DataAnnotations;

namespace CV_Flare.RazorPage.ViewModels
{
    public class EmailConfirmVM
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }
    }
}
