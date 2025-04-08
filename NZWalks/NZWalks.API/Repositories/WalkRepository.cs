using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalkRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await nZWalksDBContext.Walks.AddAsync(walk);
            await nZWalksDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetALlAsync(string? filterOn, string? filterQuery = null
            , string? sortBy = null, bool isAcsending = true
            , int pageNumber = 1, int pageSize = 1000)
        {
            var walks = nZWalksDBContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            // apply filter
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }
            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAcsending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAcsending ? walks.OrderBy(x => x.LengthInKms) : walks.OrderByDescending(x => x.LengthInKms);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();

            //return await nZWalksDBContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await nZWalksDBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKms = walk.LengthInKms;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;
            await nZWalksDBContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            return await nZWalksDBContext.Walks
                  .Include("Difficulty")
                  .Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await nZWalksDBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            nZWalksDBContext.Walks.Remove(existingWalk);
            await nZWalksDBContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
