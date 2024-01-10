using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{

    // https://localhost:7210/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Get All Region 
        // GET:https://localhost:7210/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            // Get Domain Model from Database
            var regionsDomain = dbContext.Regions.ToList();


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
        public IActionResult GetById([FromRoute] Guid id)
        {
            var region = dbContext.Regions.FirstOrDefault(r => r.Id == id);

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
        public IActionResult Create([FromBody] CreateRegionDTO createRegionDto)
        {
            // Map RegionDto to Domain Dto
            var regionDomain = new Region
            {
                Code = createRegionDto.Code,
                Name = createRegionDto.Name,
                RegionImageUrl = createRegionDto.RegionImageUrl
            };

            dbContext.Regions.Add(regionDomain);
            dbContext.SaveChanges();


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
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDto)
        {

            var regionDomain = dbContext.Regions.Find(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            regionDomain.Name = updateRegionDto.Name;
            regionDomain.Code = updateRegionDto.Code;
            regionDomain.RegionImageUrl = updateRegionDto.RegionImageUrl;

            dbContext.SaveChanges();


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


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id) 
        {
            // Fetch Domain Model from Database
            var regionDomain = dbContext.Regions.Find(id);
            if(regionDomain == null)
            {
                return NotFound();
            }
            // Delete the object of given id
            dbContext.Regions.Remove(regionDomain);
            dbContext.SaveChanges();


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
