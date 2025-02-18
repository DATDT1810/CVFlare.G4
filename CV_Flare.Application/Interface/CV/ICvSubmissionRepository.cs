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
    public interface ICvSubmissionRepository
    {
        Task<IEnumerable<CV_FLare.Domain.Models.CvSubmission>> GetAllCvSubmission();
        Task<IEnumerable<CV_FLare.Domain.Models.CvSubmission>> GetAllCvByUserId(int userId);
        Task<CV_FLare.Domain.Models.CvSubmission> GetCvSubmissionById(int id);
        Task<CV_FLare.Domain.Models.CvSubmission> GetCvSubmissionByIdandUserId(int id, int userId);
        Task<CV_FLare.Domain.Models.CvSubmission> DeleteCvSubmission(int id);
        Task<CvSubmissionDTO> SubmitCvAsync(CvSubmissionDTO submission, IFormFile file);
    }
}
