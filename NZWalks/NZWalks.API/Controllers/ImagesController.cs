using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid)
            {
                // convert DTO to domain model
                var imagemodel = new Image() 
                {
                    File=request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };
                // use repository to upload images
                await imageRepository.Upload(imagemodel);
                return Ok(imagemodel);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDTO request)
        {
            if (request == null)
            {
                var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
                if (allowedExtensions.Contains(Path.GetExtension(request.FileName)) == false)
                {
                    ModelState.AddModelError("file", "Unsupported file extensin");
                }
                if (request.File.Length > 10485760)// more than 10 mega bytes
                {
                    ModelState.AddModelError("file", "file size more than 10MB, Please upload a valid size");
                }
            }
        }
    }
}
