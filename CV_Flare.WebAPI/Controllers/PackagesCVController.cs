using AutoMapper;
using CV_Flare.Application.DTOs;
using CV_Flare.Application.Interface.PackagesCV;
using CV_FLare.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CV_Flare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesCVController : ControllerBase
    {
        private readonly IPackagesCVService _packagesCVService;
        private readonly IMapper _mapper;

        public PackagesCVController(IPackagesCVService packagesCVService, IMapper mapper)
        {
            _packagesCVService = packagesCVService;
            _mapper = mapper;
        }

        [HttpGet("GetAllPackages")]
        public async Task<IActionResult> GetAllPackagesCV()
        {
            var packages = await _packagesCVService.GetAllPackagesCV();
            return Ok(packages);
        }

        [HttpGet("{id}", Name = "GetPackagesCVById")]
        public async Task<IActionResult> GetPackagesCVById(int id)
        {
            var packagesCV = await _packagesCVService.GetPackagesCVById(id);
            if(packagesCV == null) return NotFound();
            return Ok(packagesCV);
        }

        [HttpPost]
        public async Task<IActionResult> AddPackagesCV([FromBody] PackagesDTO packagesDTO)
        {
            if (packagesDTO == null) return BadRequest();
            var newPackagesCV = _mapper.Map<Package>(packagesDTO);
            var packagesCV = await _packagesCVService.AddPackagesCV(newPackagesCV);
            return Ok(packagesCV);  
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackagesCV(int id,  [FromBody] PackagesDTO packagesDTO)
        {
            if(packagesDTO == null) return BadRequest();
            Package obj = await _packagesCVService.GetPackagesCVById(id);
            if(obj == null) return NotFound();
            var update = _mapper.Map<Package>(packagesDTO);
            await _packagesCVService.UpdatePackagesCV(id, update);
            return Ok(obj);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackagesCV(int id)
        {
            Package obj = await _packagesCVService.GetPackagesCVById(id);
            if(obj == null) return NotFound();
            await _packagesCVService.DeletePackagesCV(id);
            return NoContent();
        }

    }
}
