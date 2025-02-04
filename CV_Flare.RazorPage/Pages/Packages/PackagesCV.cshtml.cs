using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json;

namespace CV_Flare.RazorPage.Pages.Packages
{
    public class PackagesCVModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PackagesCVModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public List<PackagesVM> PackagesVM { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7000/api/PackagesCV/GetAllPackages");
            var client = _httpClientFactory.CreateClient("DefaulfClient");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Đọc nd pản hồi
                var content = await response.Content.ReadAsStringAsync();
                // Json => Oject
                PackagesVM = JsonConvert.DeserializeObject<List<PackagesVM>>(content);
            }
            else
            {
                PackagesVM = null;
            }
            return Page();
        }
    }   
}
