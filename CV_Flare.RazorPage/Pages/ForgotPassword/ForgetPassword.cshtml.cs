using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace CV_Flare.RazorPage.Pages.ForgotPassword
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ForgetPasswordModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public EmailConfirmVM EmailConfirmVM { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostCheckMail(EmailConfirmVM emailConfirmVM)
        {
            if (ModelState.IsValid)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7000/api/User/CheckEmail");
                request.Content = new StringContent(JsonConvert.SerializeObject(emailConfirmVM), Encoding.UTF8, "application/json");
                var client = _httpClientFactory.CreateClient("DefaultClient");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/ForgotPassword/ResetPassword", new { email = emailConfirmVM.Email });
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid email";
                    return Page();
                }
            }
            return Page();
        }
    }
}


