using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteByIdAsync(Guid id)
        {
            var exisitingWalk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if (exisitingWalk == null) { return null; }

            dbContext.Walks.Remove(exisitingWalk);
            await dbContext.SaveChangesAsync();
            return exisitingWalk;
        }

        public async Task<List<Walk>> GetAllAsync([FromQuery] string? filterOn = null, [FromQuery] string? filterQuery = null, [FromQuery] string? sortBy = null, [FromQuery] bool isAscending = true, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walks = dbContext.Walks.Include(p => p.Difficulty).Include(p => p.Region).AsQueryable();

            // Filter Based on Name property
            if (!string.IsNullOrEmpty(filterQuery) && !string.IsNullOrEmpty(filterOn))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(walk => walk.Name.Contains(filterQuery));
                }
            }

            // Sort Based on Name and lengthInKm
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(walk => walk.Name) : walks.OrderByDescending(walk => walk.Name);
                }
                else if (sortBy.Equals("lengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(walk => walk.LengthInKm) : walks.OrderByDescending(walk => walk.LengthInKm);
                }

            }

            // Pagination
            int skipResults = (pageNumber - 1) * pageSize;


            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var exisitingWalk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if (exisitingWalk == null) { return null; }

            return exisitingWalk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var exisitingWalk = await dbContext.Walks.FindAsync(id);
            if (exisitingWalk == null) { return null; }

            exisitingWalk.Name = walk.Name;
            exisitingWalk.WalkImageUrl = walk.WalkImageUrl;
            exisitingWalk.LengthInKm = walk.LengthInKm;
            exisitingWalk.Description = walk.Description;
            exisitingWalk.RegionID = walk.RegionID;
            exisitingWalk.DifficultyId = walk.DifficultyId;

            await dbContext.SaveChangesAsync();
            return exisitingWalk;
        }
    }
}
