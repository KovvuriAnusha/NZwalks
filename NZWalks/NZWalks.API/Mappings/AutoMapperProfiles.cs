using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequest, Region>().ReverseMap();
            CreateMap<UpdateRegionRequest, Region>().ReverseMap();

            CreateMap<AddWalkRequest, Walk>().ReverseMap();
            CreateMap<Walk,WalkDTO>().ReverseMap();
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
            CreateMap<UpdateWalkRequest, Walk>().ReverseMap();
        }
    }
}
