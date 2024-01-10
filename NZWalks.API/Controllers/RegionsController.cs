using Microsoft.AspNetCore.Mvc;
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

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        // Get All Region 
        // GET:https://localhost:7210/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            // Get Domain Model from Database
            var regionsDomain = await regionRepository.GetAllAsync();


            // Convert Domain Model to DTO Region Models and send it back
            var regionsDto = new List<RegionDto>();

            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            return Ok(regionsDto);
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

            return Ok(region);
        }



        // Create New Region
        // POST:https://localhost:7210/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRegionDTO createRegionDto)
        {
            // Map RegionDto to Domain Dto
            var regionDomain = new Region
            {
                Code = createRegionDto.Code,
                Name = createRegionDto.Name,
                RegionImageUrl = createRegionDto.RegionImageUrl
            };

            regionDomain = await regionRepository.CreateAsync(regionDomain);


            // Map DomainModel to RegionDto so as to send it to client
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }



        // Update region provided by id
        // PUT:https://localhost:7210/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDto)
        {
            // Map UpdateRegionDto to Domain Model
            var regionDomainModel = new Region
            {
                Code = updateRegionDto.Code,
                Name = updateRegionDto.Name,
                RegionImageUrl = updateRegionDto.RegionImageUrl
            };

            var regionDomain = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomain == null)
            {
                return NotFound();
            }
            // Convert Domain Model to DTO Object
            var regionDto = new RegionDto
            { 
                Id = regionDomain.Id, 
                Code = regionDomain.Code, 
                Name = regionDomain.Name, 
                RegionImageUrl = regionDomain.RegionImageUrl 
            };

            return Ok(regionDto);
        }


        // Delete region provided by id
        // DELETE:https://localhost:7210/api/regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
            // Fetch Domain Model from Database
            var regionDomain = await  regionRepository.DeleteAsync(id);
            if(regionDomain == null)
            {
                return NotFound();
            }
            // Delete the object of given id
            


            // Convert Domain Model back to DTO Model and send it back as reponse
            var regionDto = new RegionDto
            { Id = regionDomain.Id,
               
            Code =regionDomain.Code,
            Name = regionDomain.Name,
            RegionImageUrl = regionDomain.RegionImageUrl
                
            };
            return Ok(regionDto);
        }

    }
}
