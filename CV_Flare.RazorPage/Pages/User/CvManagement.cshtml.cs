using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CV_Flare.RazorPage.Pages.User
{
    public class CvManagementModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CvManagementModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [BindProperty(SupportsGet = true)]
        public IEnumerable<CvSubmissionVM> CvSubmissionVM { get; set; }

        public async Task<IActionResult> OnGet(int userId)
        {
            var id = "0";
            if (string.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }

            foreach (var cv in CvSubmissionVM)
            {
                cv.UserId = int.Parse(id);
            }
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7000/api/CvSubmission/GetAllCVSubmissionsById/{userId}");

            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                CvSubmissionVM = JsonConvert.DeserializeObject<List<CvSubmissionVM>>(content);
            }
            else
            {
                CvSubmissionVM = new List<CvSubmissionVM>();
            }
            return Page();
        }

    }
}
