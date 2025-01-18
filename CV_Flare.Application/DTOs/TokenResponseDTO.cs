using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.DTOs
{
    public class TokenResponseDTO
    {
        public string? AccessToken { get; set; }
        public TokenRefreshDTO? RefreshToken { get; set; }
    }
}
