using AutoMapper;
using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.Account;
using CV_Flare.Application.Interface.CV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UglyToad.PdfPig;
using iText.Html2pdf;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CV_Flare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CvSubmissionController : ControllerBase
    {
        private readonly ICvSubmissionService _cvSubmissionService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly string _filePath = @"D:\SP25\EXE201\CVFlare.G4\CV_Flare.WebAPI\UploadedFiles"; // Đường dẫn tuyệt đối tới thư mục UploadedFiles

        public CvSubmissionController(ICvSubmissionService cvSubmissionService, IAccountService accountService, IMapper mapper)
        {
            _cvSubmissionService = cvSubmissionService;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet("Download/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            var decodedFileName = Uri.UnescapeDataString(fileName);
            var filePath = Path.Combine(_filePath, decodedFileName);

            Debug.WriteLine($"Attempting to download file from: {filePath}");

            if (!System.IO.File.Exists(filePath))
            {
                Debug.WriteLine($"File not found at path: {filePath}");
                return NotFound("File not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", decodedFileName);
        }

        [HttpGet("ConvertToHtml")]
        public async Task<IActionResult> ConvertToHtml(string filePath)
        {
            var decodedFilePath = Uri.UnescapeDataString(filePath);
            var fullPath = Path.Combine(_filePath, decodedFilePath);

            Debug.WriteLine($"Attempting to convert file at: {fullPath}");

            if (!System.IO.File.Exists(fullPath))
            {
                Debug.WriteLine($"File not found at path: {fullPath}");
                return NotFound($"File not found at: {fullPath}");
            }

            try
            {
                string htmlContent = await ConvertPdfToHtml(fullPath);
                Debug.WriteLine("Successfully converted PDF to HTML.");
                return Content(htmlContent, "text/html");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error converting PDF to HTML: {ex.Message}");
                return StatusCode(500, new { message = "Error converting PDF to HTML", details = ex.Message });
            }
        }

        [HttpPost("SaveAndExportToPdf")]
        public async Task<IActionResult> SaveAndExportToPdf([FromBody] dynamic data)
        {
            try
            {
                string htmlContent = data.htmlContent;
                string originalFilePath = data.filePath;

                Debug.WriteLine($"Saving edited CV for file: {originalFilePath}");

                var decodedFilePath = Uri.UnescapeDataString(originalFilePath);
                var pdfBytes = ConvertHtmlToPdf(htmlContent);
                var newFileName = $"Edited_{Path.GetFileName(decodedFilePath)}";
                var newFilePath = Path.Combine(_filePath, newFileName);

                Debug.WriteLine($"Saving new PDF at: {newFilePath}");
                await System.IO.File.WriteAllBytesAsync(newFilePath, pdfBytes);

                return File(pdfBytes, "application/pdf", newFileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving and exporting PDF: {ex.Message}");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCvSubmission()
        {
            try
            {
                var list = await _cvSubmissionService.GetAllCvSubmission();
                Debug.WriteLine($"Retrieved {list?.Count() ?? 0} CV submissions.");
                return Ok(list);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving CV submissions: {ex.Message}");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("GetAllCVSubmissionsById/{userId}")]
        public async Task<IActionResult> GetAllCvSubmissionById(int userId)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _accountService.GetUserById(id);

            if (user != null)
            {
                var userID = user.UserId;
                var list = await _cvSubmissionService.GetAllCvByUserId(userId);

                if (list == null || !list.Any())
                {
                    Debug.WriteLine($"No CV submissions found for user {userId}");
                    return NotFound("No CV submissions found.");
                }

                Debug.WriteLine($"Retrieved {list.Count()} CV submissions for user {userId}");
                return Ok(list);
            }
            else
            {
                Debug.WriteLine("Invalid user ID for retrieving CV submissions.");
                return BadRequest("Invalid!");
            }
        }

        [HttpGet("{id}", Name = "GetCvSubmissionById")]
        public async Task<IActionResult> GetCvSubmissionById(int id)
        {
            var cv = await _cvSubmissionService.GetCvSubmissionById(id);
            if (cv == null)
            {
                Debug.WriteLine($"CV submission with ID {id} not found.");
                return NotFound();
            }
            return Ok(cv);
        }

        [HttpGet("{id}/{userId}", Name = "GetCvSubmissionByIdandUserId")]
        public async Task<IActionResult> GetCvSubmissionByIdandUserId(int id, int userId)
        {
            var cvId = await _cvSubmissionService.GetCvSubmissionByIdandUserId(id, userId);
            return Ok(cvId);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitCvAsync([FromForm] CvSubmissionDTO submission, [FromForm] IFormFile file)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _accountService.GetUserById(userId);

                if (user != null)
                {
                    submission.UserId = user.UserId;
                    if (file == null || file.Length == 0)
                    {
                        Debug.WriteLine("File is required but none provided.");
                        return BadRequest("File is required.");
                    }

                    var result = await _cvSubmissionService.SubmitCvAsync(submission, file);
                    Debug.WriteLine("CV submitted successfully.");
                    return Ok(result);
                }
                else
                {
                    Debug.WriteLine("Invalid user for CV submission.");
                    return BadRequest("Invalid");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error submitting CV: {ex.Message}");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        private async Task<string> ConvertPdfToHtml(string pdfPath)
        {
            using var document = PdfDocument.Open(pdfPath);
            var htmlBuilder = new System.Text.StringBuilder();
            htmlBuilder.Append("<html><body style='font-family: Arial, sans-serif; line-height: 1.6;'>");

            foreach (var page in document.GetPages())
            {
                var text = page.Text;
                htmlBuilder.Append($"<div style='margin-bottom: 20px;'>{text.Replace("\n", "<br/>")}</div>");
            }

            htmlBuilder.Append("</body></html>");
            return htmlBuilder.ToString();
        }

        private byte[] ConvertHtmlToPdf(string htmlContent)
        {
            using var memoryStream = new MemoryStream();
            HtmlConverter.ConvertToPdf(htmlContent, memoryStream);
            return memoryStream.ToArray();
        }
    }
}