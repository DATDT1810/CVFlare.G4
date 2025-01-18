using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.DTOs
{
    public class TokenRefreshDTO
    {
        public string? RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
