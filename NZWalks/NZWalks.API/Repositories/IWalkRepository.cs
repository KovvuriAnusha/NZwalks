using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk> DeleteAsync(Guid id);
        Task<List<Walk>> GetALlAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAcsending = true
            , int pageNumber = 1, int pageSize = 1000);
        Task<Walk?> GetWalkByIdAsync(Guid id);
    }
}
