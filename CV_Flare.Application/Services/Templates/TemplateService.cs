using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.Templates;
using CV_FLare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.Services.Templates
{
    public class TemplateService : ITemplatesService
    {
        private readonly ITemplatesRepository _templatesRepository;

        public TemplateService(ITemplatesRepository templatesRepository)
        {
            _templatesRepository = templatesRepository;
        }

        public async Task<Template> AddTemplates(Template template)
        {
            return await _templatesRepository.AddTemplates(template);
        }

        public async Task<Template> DeleteTemplateById(int id)
        {
            return await _templatesRepository.DeleteTemplateById(id);
        }

        public async Task<IEnumerable<Template>> GetAllTemplates()
        {
            return await _templatesRepository.GetAllTemplates();
        }

        public async Task<Template> GetTemplateById(int id)
        {
            return await _templatesRepository.GetTemplateById(id);
        }

        public async Task<Template> UpdateTemplates(int id, Template template)
        {
            return await _templatesRepository.UpdateTemplates(id, template);
        }
    }
}
