using AutoMapper;
using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.Templates;
using CV_FLare.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CV_Flare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplatesService _templateService;
        private readonly IMapper _mapper;

        public TemplatesController(ITemplatesService templateService, IMapper mapper)
        {
            _templateService = templateService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTemplates()
        {
            var templateList = await _templateService.GetAllTemplates();
            return Ok(templateList);
        }

        [HttpGet("{id}", Name = "GetTemplatesById")]
        public async Task<IActionResult> GetTemplatesById(int id)
        {
            var template = await _templateService.GetTemplateById(id);
            if (template == null) return NotFound();
            return Ok(template);
        }

        [HttpPost]
        public async Task<IActionResult> AddTemplate([FromBody] TemplatesDTO templatesDTO)
        {
            if (templatesDTO == null) return BadRequest();
            var newTemplate = _mapper.Map<Template>(templatesDTO);
            var createdTemplate = await _templateService.AddTemplates(newTemplate);
            return CreatedAtRoute("GetTemplatesById", new { id = createdTemplate.TemplateId }, createdTemplate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTemplate(int id, [FromBody] TemplatesDTO templatesDTO)
        {
            if (templatesDTO == null) return BadRequest();
            Template obj = await _templateService.GetTemplateById(id);
            if (obj == null) return NotFound();
            var updatedTemplate = _mapper.Map<Template>(templatesDTO);
            updatedTemplate.TemplateId = id;
            await _templateService.UpdateTemplates(id, updatedTemplate);
            return Ok(obj);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            var template = await _templateService.GetTemplateById(id);
            if (template == null) return NotFound($"Template with ID {id} not found.");

            await _templateService.DeleteTemplateById(id);
            return NoContent();
        }
    }
}
