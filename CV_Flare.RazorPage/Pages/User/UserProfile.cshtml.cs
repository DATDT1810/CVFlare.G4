using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CV_Flare.RazorPage.Pages.User
{
    public class UserProfileModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserProfileModel(IHttpClientFactory httpClientFactory)
        {
          _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public UserProfileVM? UserProfileVM { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7000/api/User/GetUserProfile");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var userProfile = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(userProfile)){
                    UserProfileVM = JsonConvert.DeserializeObject<UserProfileVM>(userProfile);
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(UserProfileVM userProfileVM)
        {
            if (ModelState.IsValid)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7000/api/User/UpdateUserProfile");
                request.Content = new StringContent(JsonConvert.SerializeObject(UserProfileVM), Encoding.UTF8, "application/json");
                var client = _httpClientFactory.CreateClient("DefaultClient");
                client.Timeout = TimeSpan.FromMinutes(5);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["message"] = "true";
                }
                else
                {
                    TempData["message"] = "false";
                }
            }
            else
            {
                UserProfileVM ??= new UserProfileVM();
            }
            return Page();
        }

    }
}
