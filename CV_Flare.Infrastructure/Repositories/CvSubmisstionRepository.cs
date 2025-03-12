using AutoMapper;
using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.CV;
using CV_Flare.Infrastructure.DB;
using CV_FLare.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Infrastructure.Repositories
{
    public class CvSubmisstionRepository : ICvSubmissionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _fileStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        public CvSubmisstionRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            if (!Directory.Exists(_fileStoragePath))
            {
                Directory.CreateDirectory(_fileStoragePath);
            }
        }

        public async Task<CV_FLare.Domain.Models.CvSubmission> DeleteCvSubmission(int id)
        {
            var cvId = await _context.CvSubmissions.FirstOrDefaultAsync(c => c.SubmissionId == id);
            if (cvId != null)
            {
                _context.CvSubmissions.Remove(cvId);
                await _context.SaveChangesAsync();
            }
            throw new Exception();
        }

        public async Task<IEnumerable<CV_FLare.Domain.Models.CvSubmission>> GetAllCvByUserId(int userId)
        {
            return await _context.CvSubmissions
            .Where(cv => cv.UserId == userId)
            .ToListAsync();

        }

        public async Task<IEnumerable<CV_FLare.Domain.Models.CvSubmission>> GetAllCvSubmission()
        {
            return await _context.CvSubmissions.Include(u => u.User).ToListAsync();
        }

        public async Task<CV_FLare.Domain.Models.CvSubmission> GetCvSubmissionById(int id)
        {
            var cvSubmission = await _context.CvSubmissions.FirstOrDefaultAsync(c => c.SubmissionId == id);
            if (cvSubmission == null)
                throw new KeyNotFoundException($"CV Submission with ID {id} not found.");
            return cvSubmission;
        }


        public async Task<CV_FLare.Domain.Models.CvSubmission> GetCvSubmissionByIdandUserId(int id, int userId)
        {
            var cvId = await _context.CvSubmissions.FindAsync(id, userId);
            if (cvId == null) throw new Exception(); return cvId;
        }

        public async Task<CvSubmissionDTO> SubmitCvAsync(CvSubmissionDTO submission, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(_fileStoragePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                submission.FilePath = $"/UploadedFiles/{fileName}";
            }

            var entity = _mapper.Map<CV_FLare.Domain.Models.CvSubmission>(submission);
            entity.Status = "Submitted";
            entity.AiScore = 0;
            entity.UploadedAt = DateTime.UtcNow;

             _context.CvSubmissions.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<CvSubmissionDTO>(entity);
        }

    }


}
