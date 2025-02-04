using CV_FLare.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.Interface.CV
{
    public interface IUploadCVRepository
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task<IEnumerable<CvSubmission>> GetFiles();
        byte[] GetFile(string filename);
        void DeleteFile(string filename);
    }
}
