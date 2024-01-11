using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{

    // https://localhost:7210/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // Get All Region 
        // GET:https://localhost:7210/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            // Get Domain Model from Database
            var regionsDomain = await regionRepository.GetAllAsync();

            // Convert Domain Model to DTO Region Models and send 
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }


        // Get Single Region ( Get region By Id )
        // GET:https://localhost:7210/api/regions/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await regionRepository.GetByIdAsync(id);

            // This is used to find region by id only
            //var region2 = dbContext.Regions.Find(id);
            if (region == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(region));
        }



        // Create New Region
        // POST:https://localhost:7210/api/regions
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CreateRegionDTO createRegionDto)
        {
            if (ModelState.IsValid)
            {
                // Map CreateRegionDto to RegionDomain 
                var regionDomain = mapper.Map<Region>(createRegionDto);

                regionDomain = await regionRepository.CreateAsync(regionDomain);

                // Map DomainModel to RegionDto 
                var regionDto = mapper.Map<RegionDto>(regionDomain);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }
            else
            {
                return BadRequest(ModelState);
            };
        }



        // Update region provided by id
        // PUT:https://localhost:7210/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDto)
        {
            // Map UpdateRegionDto to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionDto);

            var regionDomain = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomain == null)
            {
                return NotFound();
            }
            // Convert Domain Model to DTO Object
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        // Delete region provided by id
        // DELETE:https://localhost:7210/api/regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Fetch Domain Model from Database
            var regionDomain = await regionRepository.DeleteAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            // Convert Domain Model back to DTO Model and send it back as 
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

    }
}
