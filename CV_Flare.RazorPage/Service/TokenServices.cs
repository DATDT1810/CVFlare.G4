using CV_Flare.RazorPage.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace CV_Flare.RazorPage.Service
{
    public class TokenServices
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string accessToken = "";
        private string refreshToken = "";

        public TokenServices(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetAccessToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"] ?? string.Empty;
        }
        public RefreshToken GetRefreshToken()
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"] ?? string.Empty;
            var refreshTokenExpiration = _httpContextAccessor.HttpContext?.Request.Cookies["refreshTokenExpiration"] ?? string.Empty;
            return new RefreshToken
            {
                refreshToken = refreshToken,
                Expiration = DateTime.Parse(refreshTokenExpiration)
            };
        }

        public async Task SetTokens(string accessToken, RefreshToken refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SameSite = SameSiteMode.None,
            };
            // Thiết lập cookie  refreshToken
            var cookieOptions2 = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(1),
                SameSite = SameSiteMode.None,
            };
            // Thiết lập cookie cho accessToken và refreshToken
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", accessToken, cookieOptions);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken.refreshToken, cookieOptions2);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshTokenExpiration", refreshToken.Expiration.ToString(), cookieOptions2);
            }
        }

        public async Task<TokenResponse> RefreshToken(RefreshToken refreshToken)
        {
            if (refreshToken == null)
            {
                return null;
            }
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/RefreshToken");
            request.Content = new StringContent(JsonConvert.SerializeObject(new { refreshToken }), Encoding.UTF8, "application/json");
            var client = httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(token);
                if (tokenResponse?.AccessToken != null && tokenResponse.RefreshToken != null)
                {
                    SetTokens(tokenResponse.AccessToken, tokenResponse.RefreshToken);
                    return tokenResponse; // Trả về access token mới
                }
            }
            DeleteTokens();
            return null;
        }
        public void DeleteTokens()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("accessToken");
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshTokenExpiration");
            }
        }
    }
}
