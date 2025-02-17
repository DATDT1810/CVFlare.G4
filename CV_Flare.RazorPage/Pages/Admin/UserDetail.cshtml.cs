using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CV_Flare.RazorPage.Pages.Admin
{
    public class UserDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public UserProfileVM list { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7000/api/User/{id}");
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var reponse = await client.SendAsync(request);
            if (reponse.IsSuccessStatusCode)
            {
                var content = await reponse.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<UserProfileVM>(content);
            }
            else
            {
                list = null;
            }
            return Page();
        }
    }
}

