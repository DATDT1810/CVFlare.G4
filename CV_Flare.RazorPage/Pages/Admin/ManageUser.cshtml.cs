using CV_Flare.Application.Interface.Account;
using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CV_Flare.RazorPage.Pages.Admin
{
   
    public class ManageUserModel : PageModel
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public ManageUserModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public List<UserProfileVM> list { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7000/api/User");
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var reponse = await client.SendAsync(request);
            if (reponse.IsSuccessStatusCode)
            {
                var content = await reponse.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<UserProfileVM>>(content);
            }
            else
            {
                list = null;
            }
            return Page();
        }
    }
}
