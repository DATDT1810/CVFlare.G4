using AutoMapper;
using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.Account;
using CV_Flare.Application.Interface.CV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CV_Flare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CvSubmisstionController : ControllerBase
    {
        private readonly ICvSubmissionService _cvSubmissionService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CvSubmisstionController(ICvSubmissionService cvSubmissionService, IAccountService accountService, IMapper mapper)
        {
            _cvSubmissionService = cvSubmissionService;
            _accountService = accountService;
            _mapper = mapper;
        }
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCvSubmission()
        {
            var list = await _cvSubmissionService.GetAllCvSubmission();
            return Ok(list);
        }

        [Authorize]
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
                    return NotFound("No CV submissions found.");
                }

                return Ok(list);
            }
            else
            {
                return BadRequest("Invalid!");
            }
        }

        //[Authorize]
        [HttpGet("{id}", Name = "GetCvSubmissionById")]
        public async Task<IActionResult> GetCvSubmissionById(int id)
        {
            var cv = await _cvSubmissionService.GetCvSubmissionById(id);
            if(cv == null) return NotFound();
            return Ok(cv);
        }

        //[Authorize]
        [HttpGet("{id}/{userId}", Name = "GetCvSubmissionByIdandUserId")]
        public async Task<IActionResult> GetCvSubmissionByIdandUserId(int id, int userId)
        {
            var cvId = await _cvSubmissionService.GetCvSubmissionByIdandUserId(id, userId);
            return Ok(cvId);
        }

        [Authorize]
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitCvAsync([FromForm] CvSubmissionDTO submission, [FromForm] IFormFile file)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _accountService.GetUserById(userId);

                if(user != null)
                {
                    submission.UserId = user.UserId;
                    if (file == null || file.Length == 0)
                    {
                        return BadRequest("File is required.");
                    }

                    var result = await _cvSubmissionService.SubmitCvAsync(submission, file);
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Invalid");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
