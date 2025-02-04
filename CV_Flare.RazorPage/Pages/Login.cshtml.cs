using CV_Flare.RazorPage.Service;
using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace CV_Flare.RazorPage.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly TokenServices _tokenServices;

        public LoginModel(IHttpClientFactory clientFactory, TokenServices tokenServices)
        {
            _clientFactory = clientFactory;
            _tokenServices = tokenServices;
        }

        [BindProperty]
        public LoginVM LoginVM { get; set; } = new LoginVM();
        [BindProperty(SupportsGet = true)]
        public string UrlPageFrom { get; set; } = "/Index";
        public async Task<IActionResult> OnGet()
        {
            var urlFrom = Request.Query["Url"].ToString();
            if (!string.IsNullOrEmpty(urlFrom))
            {
                UrlPageFrom = urlFrom;
            }
            return Page();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(LoginVM.email))
            {
                ModelState.AddModelError("LoginVM.email", "Email cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(LoginVM.password))
            {
                ModelState.AddModelError("LoginVM.password", "Password cannot be empty.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7000/api/User/Login")
            {
                Content = new StringContent(JsonConvert.SerializeObject(LoginVM), Encoding.UTF8, "application/json")
            };
            var client = _clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2);

            try
            {
                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("LoginVM.email", "Invalid email or password.");
                    return Page();
                }

                var token = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(token);
                if (tokenResponse?.AccessToken == null || tokenResponse?.RefreshToken == null)
                {
                    ModelState.AddModelError("LoginVM.email", "Invalid login attempt.");
                    return Page();
                }

                await _tokenServices.SetTokens(tokenResponse.AccessToken, tokenResponse.RefreshToken);
                TempData["Success"] = "true";
                return RedirectToPage(UrlPageFrom);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Network error. Please try again.");
                return Page();
            }
        }


        public IActionResult OnGetLoginWithGoogle()
        {
            var redirectUrl = Url.Page("/LoginGoogle");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return new ChallengeResult("Google", properties);
        }
    }
}
