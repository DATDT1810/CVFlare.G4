using System.ComponentModel.DataAnnotations;

namespace CV_Flare.RazorPage.ViewModels
{
    public class ResetPasswordVM
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("password", ErrorMessage = "Passwords do not match")]
        public string confirmPassword { get; set; }
    }
}
