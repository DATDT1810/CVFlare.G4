using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CV_Flare.RazorPage.Pages.ForgotPassword
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ResetPasswordModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public ResetPasswordVM ResetPasswordVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }
        public IActionResult OnGet()
        {
            Email = Request.Query["email"];
            return Page();
        }

        public async Task<IActionResult> OnPost(ResetPasswordVM resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                return RedirectToPage("/ForgotPassword/ConfirmCode", new { email = Email, newPassword = ResetPasswordVM.password });
            }
            return Page();
        }
    }
}
