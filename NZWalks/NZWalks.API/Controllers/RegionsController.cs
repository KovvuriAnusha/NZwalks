using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext _dBContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDBContext nZWalksDBContext, IRegionRepository regionRepository
            , IMapper mapper, ILogger<RegionsController> logger)
        {
            this._dBContext = nZWalksDBContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        // Get All Regions
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAllRegions()
        {

            try
            {
                var regions = await regionRepository.GetAllRegionsAsync();
                //var regionsDTO = new List<RegionDTO>();
                //foreach (var region in regions)
                //{
                //    var regionDTO = new RegionDTO()
                //    {
                //        Id = region.Id,
                //        Code = region.Code,
                //        Name = region.Name,
                //        RegionImageUrl = region.RegionImageUrl
                //    };
                //    regionsDTO.Add(regionDTO);
                //}
                logger.LogInformation($"Finished Get all regions data : {JsonSerializer.Serialize(regions)}");
                return Ok(mapper.Map<List<RegionDTO>>(regions));
               
            }
            catch (Exception ex)
            {
                logger.LogError(ex,ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // Get Region by id
        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await regionRepository.GetRegionByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            //var regionDTO = new RegionDTO()
            //{
            //    Id = region.Id,
            //    Code = region.Code,
            //    Name = region.Name,
            //    RegionImageUrl = region.RegionImageUrl
            //};
            // 
            return Ok(mapper.Map<RegionDTO>(region));
        }

        // POST to create Region
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequest request)
        {
            try
            {
                //var regionDomainModel = new Region()
                //{
                //    Code = request.Code,
                //    Name = request.Name,
                //    RegionImageUrl = request.RegionImageUrl
                //};
                var regionDomainModel = mapper.Map<Region>(request);
                await regionRepository.CreateRegionAsync(regionDomainModel);
                //var regionDto = new RegionDTO() { Id = regionDomainModel.Id, Code = regionDomainModel.Code, Name = regionDomainModel.Name, RegionImageUrl = regionDomainModel.RegionImageUrl };
                var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // PUT to update Region
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequest request)
        {
            //var regionDomainModel = new Region()
            //{
            //    Code = request.Code,
            //    Name = request.Name,
            //    RegionImageUrl = request.RegionImageUrl,
            //};
            var regionDomainModel = mapper.Map<Region>(request);
            var region = await regionRepository.UpdateRegionAsync(id, regionDomainModel);
            if (region == null)
            {
                return NotFound();
            }
            //var regionDTO = new RegionDTO() { Id = region.Id, Code = region.Code, Name = region.Name, RegionImageUrl = region.RegionImageUrl };
            var regionDTO = mapper.Map<RegionDTO>(region);
            return Ok(regionDTO);
        }

        // Delete Region
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await regionRepository.DeleteRegionAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            //var regionDTO = new RegionDTO() { Id = region.Id, Code = region.Code, Name = region.Name, RegionImageUrl = region.RegionImageUrl };
            var regionDTO = mapper.Map<RegionDTO>(region);
            return Ok(regionDTO);
        }
    }
}
