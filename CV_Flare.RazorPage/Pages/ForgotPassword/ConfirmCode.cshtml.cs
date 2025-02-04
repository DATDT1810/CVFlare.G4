using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace CV_Flare.RazorPage.Pages.ForgotPassword
{
    public class ConfirmCodeModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ConfirmCodeModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [BindProperty(SupportsGet = true)]
        public string Code { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Password { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Email = Request.Query["email"];
            Password = Request.Query["newPassword"];
            var reqest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7000/api/User/SendCode");
            var content = new
            {
                Email = Email
            };
            reqest.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.SendAsync(reqest);
            if(response.IsSuccessStatusCode)
            {
                return Page();
            }
            Message = "Error";
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var changePassword = new ChangePasswordVM
            {
                password = Password,
                code = Code,
                email = Email
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7000/api/User/ResetPassword");
            request.Content = new StringContent(JsonConvert.SerializeObject(changePassword), Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = true;
                return RedirectToPage("/Login");
            }
            return Page();
        }
    }
}
