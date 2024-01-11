using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository repository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // Create Walks
        // POST:https://localhost:7210/api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]CreateWalkDTO createWalkDTO)
        {
            var walkDomain = mapper.Map<Walk>(createWalkDTO);
            walkDomain = await repository.CreateAsync(walkDomain);

            return Ok(mapper.Map<WalkDto>(walkDomain));
        }


        // Get Walks By Id
        // GET:https://localhost:7210/api/walks/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomain = await repository.GetByIdAsync(id);

            if(walkDomain == null) { return NotFound(); }

            return Ok(mapper.Map<WalkDto>(walkDomain));

        }

        // Get All Walks
        // GET:https://localhost:7210/api/walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomain = await repository.GetAllAsync();
            return Ok(mapper.Map<List<WalkDto>>(walksDomain));
        }

        // Update the walks by id
        // PUT:https://localhost:7210/api/walks/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateWalkDTO updateWalkDTO)
        {
            var walkDomain = await repository.UpdateAsync(id,mapper.Map<Walk>(updateWalkDTO));
            if (walkDomain==null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomain));

        }


        // Delete Walks By Id
        // GET:https://localhost:7210/api/walks/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var walkDomain = await repository.DeleteByIdAsync(id);

            if (walkDomain == null) { return NotFound(); }

            return Ok(mapper.Map<WalkDto>(walkDomain));

        }
    }
}
