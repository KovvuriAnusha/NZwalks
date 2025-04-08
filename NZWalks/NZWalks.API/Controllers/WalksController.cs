using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly NZWalksDBContext _dBContext;
        private readonly IMapper mapper;
        public readonly IWalkRepository walkRepository;
        public WalksController(NZWalksDBContext nZWalksDBContext, IMapper mapper, IWalkRepository walkRepository)
        {
            this._dBContext = nZWalksDBContext;
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // Create Walk
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequest addWalkRequest)
        {
            var walk = mapper.Map<Walk>(addWalkRequest);
            await walkRepository.CreateAsync(walk);
            return Ok(mapper.Map<WalkDTO>(walk));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery
            , [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walks = await walkRepository.GetALlAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            // Create an exception
            throw new Exception("This is is new exception");
            return Ok(mapper.Map<List<WalkDTO>>(walks));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walk = await walkRepository.GetWalkByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDTO>(walk));
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequest updateWalkRequest)
        {

            var walk = mapper.Map<Walk>(updateWalkRequest);
            walk = await walkRepository.UpdateAsync(id, walk);
            if (walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDTO>(walk));

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walk = await walkRepository.DeleteAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDTO>(walk));
        }
    }
}
