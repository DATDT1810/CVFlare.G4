using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CV_Flare.RazorPage.ViewModels;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CV_Flare.RazorPage.Pages
{
    public class UploadCVModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UploadCVModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }

        [BindProperty(SupportsGet =true)]
        public CvSubmissionVM CvSubmissionVM { get; set; } = new CvSubmissionVM();

        [BindProperty(SupportsGet =true)]
        public PackagesVM PackagesVM { get; set; }

        [BindProperty]
        public IFormFile CVFile { get; set; }

        [FromQuery]
        public int PackageId { get; set; }

        public async Task<IActionResult> OnGet(int packageId)
        {
            PackageId = packageId;
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7000/api/PackagesCV/{packageId}");
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                PackagesVM = JsonConvert.DeserializeObject<PackagesVM>(content);
            }
            else
            {
                PackagesVM = null;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(CvSubmissionVM.JobDescripion))
            {
                ViewData["CVStatus"] = "Mô tả công việc không được để trống.";
                TempData["Success"] = false;
                return Page();
            }

            if (CVFile == null || CVFile.Length == 0)
            {
                ViewData["CVStatus"] = "Vui lòng chọn một file PDF.";
                return Page();
            }

            var userId = "0";
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            CvSubmissionVM.UserId = int.Parse(userId);
            CvSubmissionVM.PackageId = PackageId;

            // Create HttpClient and set timeout before using
            var client = _httpClientFactory.CreateClient("DefaultClient");

            client.Timeout = TimeSpan.FromMinutes(2); // Set timeout here
            using (var content = new MultipartFormDataContent())
            {
                // Thêm các trường dữ liệu cần thiết (đảm bảo key trùng khớp với DTO của API)
                content.Add(new StringContent(CvSubmissionVM.UserId.ToString()), "UserId");
                content.Add(new StringContent(CvSubmissionVM.PackageId.ToString()), "PackageId");
                content.Add(new StringContent(CvSubmissionVM.JobDescripion), "JobDescripion");
                content.Add(new StringContent("Submitted"), "Status");

                // Gửi file với key "file" theo yêu cầu của API
                var fileStream = CVFile.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                content.Add(fileContent, "file", CVFile.FileName);

                var response = await client.PostAsync("https://localhost:7000/api/CvSubmisstion/submit", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = true;
                    ViewData["CVStatus"] = "Tải lên CV thành công!";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ViewData["CVStatus"] = $"Lỗi: {errorMessage}";
                }
            }

            return Page();
        }
    }
}
