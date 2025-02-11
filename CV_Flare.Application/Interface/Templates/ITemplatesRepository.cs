using CV_Flare.Application.DTOs;
using CV_FLare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.Interface.Templates
{
    public interface ITemplatesRepository
    {
        Task<IEnumerable<Template>> GetAllTemplates();
        Task<Template> GetTemplateById(int id);
        Task<Template> AddTemplates(Template template);
        Task<Template> UpdateTemplates(int id, Template template);
        Task<Template> DeleteTemplateById(int id);

    }
}
