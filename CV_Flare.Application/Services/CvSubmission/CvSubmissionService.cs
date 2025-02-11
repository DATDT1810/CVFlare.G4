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

        public async Task<CvSubmissionDTO> SubmitCvAsync(CvSubmissionDTO submission, IFormFile file)
        {
            //var hasPackage = await _userPackageRepository.HasPurchasedPackage(submission.UserId, submission.PackageId);
            //if (!hasPackage)
            //{
            //    throw new Exception("User has not purchased this package.");
            //}
            return await _cvSubmissionRepository.SubmitCvAsync(submission, file);
        }
    }
}
