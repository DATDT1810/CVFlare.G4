using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.CV;
using CV_Flare.Application.Interface.Templates;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.Services.CvSubmission
{
    public class CvSubmissionService : ICvSubmissionService
    {
        private readonly ICvSubmissionRepository _cvSubmissionRepository;

        public CvSubmissionService(ICvSubmissionRepository cvSubmissionRepository)
        {
            _cvSubmissionRepository = cvSubmissionRepository;
        }

        public async Task<CV_FLare.Domain.Models.CvSubmission> DeleteCvSubmission(int id)
        {
            return await _cvSubmissionRepository.DeleteCvSubmission(id);
        }

        public async Task<IEnumerable<CV_FLare.Domain.Models.CvSubmission>> GetAllCvByUserId(int userId)
        {
            return await _cvSubmissionRepository.GetAllCvByUserId(userId);
        }

        public async Task<IEnumerable<CV_FLare.Domain.Models.CvSubmission>> GetAllCvSubmission()
        {
            return await _cvSubmissionRepository.GetAllCvSubmission();
        }

        public async Task<CV_FLare.Domain.Models.CvSubmission> GetCvSubmissionById(int id)
        {
            return await _cvSubmissionRepository.GetCvSubmissionById(id);
        }

        public async Task<CV_FLare.Domain.Models.CvSubmission> GetCvSubmissionByIdandUserId(int id, int userId)
        {
            return await _cvSubmissionRepository.GetCvSubmissionByIdandUserId(id, userId);
        }

        public async Task<CvSubmissionDTO> SubmitCvAsync(CvSubmissionDTO submission, IFormFile file)
        {
            return await _cvSubmissionRepository.SubmitCvAsync(submission, file);
        }
    }
}
