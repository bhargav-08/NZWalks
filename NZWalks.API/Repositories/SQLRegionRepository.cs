using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var regionDomainModel = await dbContext.Regions.FindAsync(id);


            if (regionDomainModel == null) { return null; }

            // NOTE: There is not RemoveAsync like SaveChangesAsync,AddAsync etc....
            dbContext.Regions.Remove(regionDomainModel);
            await dbContext.SaveChangesAsync();
            return regionDomainModel;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id ,Region region)
        {
            var exisitingRegion = await dbContext.Regions.FindAsync(id);
            if (exisitingRegion == null) { return null; }

            exisitingRegion.Name = region.Name;
            exisitingRegion.Code = region.Code;
            exisitingRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            return exisitingRegion;
        }
    }
}
