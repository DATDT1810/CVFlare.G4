using System.ComponentModel.DataAnnotations;

namespace CV_Flare.RazorPage.ViewModels
{
    public class UserProfileVM
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        public string UserFullname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string UserPhone { get; set; }

        public string UserImg { get; set; }
    }
}
