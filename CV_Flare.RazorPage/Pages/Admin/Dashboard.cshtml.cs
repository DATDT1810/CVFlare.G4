using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CV_Flare.RazorPage.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public List<CvSubmissionVM> CvSubmissionVM { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7000/api/CvSubmission");
            var client = _httpClientFactory.CreateClient("DefaultClient");

            try
            {
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    CvSubmissionVM = JsonConvert.DeserializeObject<List<CvSubmissionVM>>(content);
                    if (CvSubmissionVM == null)
                    {
                        CvSubmissionVM = new List<CvSubmissionVM>();
                    }
                    Debug.WriteLine($"Successfully retrieved {CvSubmissionVM.Count} CV submissions.");
                }
                else
                {
                    CvSubmissionVM = new List<CvSubmissionVM>();
                    ModelState.AddModelError(string.Empty, $"Lỗi khi gọi API: {response.StatusCode} - {response.ReasonPhrase}");
                    Debug.WriteLine($"API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                CvSubmissionVM = new List<CvSubmissionVM>();
                ModelState.AddModelError(string.Empty, $"Lỗi hệ thống: {ex.Message}");
                Debug.WriteLine($"Exception: {ex.Message}");
            }

            return Page();
        }
    }
}