using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.Templates;
using CV_Flare.Infrastructure.DB;
using CV_FLare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Infrastructure.Repositories
{
    public class TemplatesRepository : ITemplatesRepository
    {
        private readonly ApplicationDbContext _context;

        public TemplatesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Template> AddTemplates(Template template)
        {
            _context.Add(template);
            await _context.SaveChangesAsync();
            return template;

        }

        public async Task<Template> DeleteTemplateById(int id)
        {
            var template = await _context.Templates.FindAsync(id);
            if (template != null)
            {
                _context.Remove(template);
                await _context.SaveChangesAsync();
                return template;
            }
            throw new KeyNotFoundException($"Tmeplate with ID {id} not found.");
        }

        public async Task<IEnumerable<Template>> GetAllTemplates()
        {
            return await _context.Templates.ToListAsync();
        }

        public async Task<Template> GetTemplateById(int id)
        {
            var template = await _context.Templates.FindAsync(id);
            if (template == null)
            {
                throw new KeyNotFoundException($"Tmeplate with ID {id} not found.");
            }
            return template;
        }

        public async Task<Template> UpdateTemplates(int id, Template template)
        {
            var existingTemplate = await _context.Templates.FindAsync(id);
            if (existingTemplate != null)
            {
                existingTemplate.TemplateName = template.TemplateName;
                existingTemplate.PreviewImg = template.PreviewImg;
                existingTemplate.TemplateFile = template.TemplateFile;
                existingTemplate.UpdateAt = DateTime.Now;
                _context.Update(existingTemplate);
                await _context.SaveChangesAsync();
                return existingTemplate;
            }
            throw new KeyNotFoundException($"Tmeplate with ID {id} not found.");
        }
    }
}
