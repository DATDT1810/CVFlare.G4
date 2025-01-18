using Jose;
using System.Text.Json;

namespace CV_Flare.RazorPage.Helper
{
    public class JwtHelper
    {
        private const int timeStamp = 300; // 5 phút
        public static bool IsTokenExpired(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            }
            try
            {
                var payload = JWT.Payload(token);
                var tokenData = JsonSerializer.Deserialize<JwtPayload>(payload);

                if (tokenData != null)
                {
                    var expirationTime = DateTimeOffset.FromUnixTimeSeconds(tokenData.exp).DateTime;
                    var timeRemaining = expirationTime - DateTime.UtcNow;

                    return timeRemaining.TotalSeconds <= timeStamp;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error processing token.", ex);
            }

            return true;
        }

    }
    public class JwtPayload
    {
        public long exp { get; set; }
    }
}
