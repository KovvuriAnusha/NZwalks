using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext _dBContext;
        public RegionRepository(NZWalksDBContext nZWalksDBContext)
        {
            this._dBContext = nZWalksDBContext;
        }

        public async Task<List<Region>> GetAllRegionsAsync()
        {
            return await _dBContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetRegionByIdAsync(Guid Id)
        {
            return await _dBContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Region> CreateRegionAsync(Region region)
        {
            await _dBContext.Regions.AddAsync(region);
            await _dBContext.SaveChangesAsync();
            return region;
        }
        public async Task<Region?> UpdateRegionAsync(Guid id, Region region)
        {
            var existingRegion = await _dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;
            await _dBContext.SaveChangesAsync();
            return existingRegion;
        }
        public async Task<Region?> DeleteRegionAsync(Guid id)
        {
            var region = await _dBContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return null;
            }
            _dBContext.Regions.Remove(region);
            await _dBContext.SaveChangesAsync();
            return region;
        }

    }
}
