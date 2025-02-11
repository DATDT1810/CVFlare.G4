using CV_Flare.Application.DTOs;
using CV_FLare.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.Interface.CV
{
    public interface ICvSubmissionService
    {
        //Task<CvSubmissionDTO> UploadJobDescription(int userId, string jobDescriptionText);
        //Task<CvSubmissionDTO> UploadCV(int userId, int packageId, IFormFile cvFile);
        Task<CvSubmissionDTO> SubmitCvAsync(CvSubmissionDTO submission, IFormFile file);
    }
}
