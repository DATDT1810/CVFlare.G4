using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.DTOs
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public string UserFullname { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string? UserImg { get; set; }
    }
}
