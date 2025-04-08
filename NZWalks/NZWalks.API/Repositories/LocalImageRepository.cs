using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment environment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDBContext dBContext;

        public LocalImageRepository(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor, NZWalksDBContext dBContext)
        {
            this.environment = environment;
            this.httpContextAccessor = httpContextAccessor;
            this.dBContext = dBContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(environment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");
            // Upload image to local path i.e., we have images folder 
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https://localhost:1234/images/image.png
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            // Add Image to Images Table
            await dBContext.Images.AddAsync(image);
            await dBContext.SaveChangesAsync();
            return image;
        }
    }
}
