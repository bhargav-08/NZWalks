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

        public async Task<List<Walk>> GetAllAsync()
        {
           return await dbContext.Walks.Include(p=>p.Difficulty).Include(p=>p.Region).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var exisitingWalk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x=>x.Id == id);
            if (exisitingWalk == null) { return null; }

            return exisitingWalk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var exisitingWalk = await dbContext.Walks.FindAsync(id);
            if(exisitingWalk == null) { return null; }

            exisitingWalk.Name = walk.Name;
            exisitingWalk.WalkImageUrl = walk.WalkImageUrl;
            exisitingWalk.LengthInKm = walk.LengthInKm;
            exisitingWalk.Description = walk.Description;
            exisitingWalk.RegionID = walk.RegionID;
            exisitingWalk.DifficultyId  = walk.DifficultyId;

            await dbContext.SaveChangesAsync();
            return exisitingWalk;
        }
    }
}
