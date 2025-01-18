using CV_Flare.RazorPage.Service;
using System.Net.Http.Headers;
using System.Net;

namespace CV_Flare.RazorPage.Helper
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly TokenServices _tokenService;

        public TokenHandler(TokenServices tokenService)
        {
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Gắn token hiện tại vào mỗi request
            var accessToken = _tokenService.GetAccessToken();
            if (!string.IsNullOrEmpty(accessToken))
            {
                if (JwtHelper.IsTokenExpired(accessToken))
                {
                    // Nếu token đã hết hạn, lấy refresh token và làm mới token
                    var refreshToken = _tokenService.GetRefreshToken();
                    var tokenRefreshed = await _tokenService.RefreshToken(refreshToken);

                    // Nếu làm mới thành công, lấy token mới
                    if (tokenRefreshed != null)
                    {
                        accessToken = _tokenService.GetAccessToken();
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }
                }
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            // Status (401), thực hiện làm mới token
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    var refreshToken = _tokenService.GetRefreshToken();
                    var tokenRefreshed = await _tokenService.RefreshToken(refreshToken);
                    if (tokenRefreshed != null)
                    {
                        // Clone request và gắn lại token mới
                        request = CloneHttpRequestMessage(request);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetAccessToken());
                        response = await base.SendAsync(request, cancellationToken);
                    }
                    else
                    {
                        _tokenService.DeleteTokens();
                        response.Content = new StringContent("/Login");
                    }
                }
                catch (Exception)
                {
                    _tokenService.DeleteTokens();
                    response.Content = new StringContent("/Login");
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                response.Content = new StringContent("/Home/404");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                response.Content = new StringContent("/Home/404");
            }
            return response;
        }
        private HttpRequestMessage CloneHttpRequestMessage(HttpRequestMessage req)
        {
            var clone = new HttpRequestMessage(req.Method, req.RequestUri)
            {
                Content = req.Content != null ? new StreamContent(req.Content.ReadAsStream()) : null,
                Version = req.Version
            };

            // Sao chép các header
            foreach (var header in req.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            // Sao chép các header của content (nếu có)
            if (req.Content != null)
            {
                foreach (var header in req.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return clone;
        }

    }
}
