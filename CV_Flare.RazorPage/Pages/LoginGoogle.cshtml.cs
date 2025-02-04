using CV_Flare.RazorPage.Service;
using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace CV_Flare.RazorPage.Pages
{
    public class LoginGoogleModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenServices _tokenServices;

        public LoginGoogleModel(IHttpClientFactory httpClientFactory, TokenServices tokenServices)
        {
            _httpClientFactory = httpClientFactory;
            _tokenServices = tokenServices;
        }

        public async Task<IActionResult> Onget()
        {
            var result = await HttpContext.AuthenticateAsync();

            if (!result.Succeeded)
            {
                return RedirectToPage("/Error");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var googleUserEmail = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Gửi thông tin này tới ASP.NET API để xác thực với Identity
            if (googleUserEmail != null)
            {
                var token = await AuthenticateWithApi(googleUserEmail);
                if (token?.AccessToken != null && token?.RefreshToken != null)
                {
                    await _tokenServices.SetTokens(token.AccessToken, token.RefreshToken);
                }
            }
            await Task.Delay(1000);
            return RedirectToPage("/Index");
        }

        private async Task<TokenResponse> AuthenticateWithApi(string googleEmail)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7000/api/User/LoginWithGoogle");
            request.Content = new StringContent(JsonConvert.SerializeObject(new { email = googleEmail }), Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            try
            {
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(token);
                    if (tokenResponse != null)
                    {
                        return tokenResponse;
                    }
                }
                throw new Exception("Invalid login attempt.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return null;
            }
        }
    }
}
