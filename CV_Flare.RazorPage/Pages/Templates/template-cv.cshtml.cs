using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CV_Flare.RazorPage.Pages.Templates
{
    public class template_cvModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public template_cvModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public List<TemplatesVM> TemplatesVM { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7000/api/Templates");
            var client = _httpClientFactory.CreateClient("DefaulfClient");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Đọc nd pản hồi
                var content = await response.Content.ReadAsStringAsync();
                // Json => Oject
                TemplatesVM = JsonConvert.DeserializeObject<List<TemplatesVM>>(content);
            }
            else
            {
                TemplatesVM = null;
            }
            return Page();
        }
    }
}
