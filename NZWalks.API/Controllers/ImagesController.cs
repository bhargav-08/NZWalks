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
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid)
            {
                // Convert DTO to Domain Image Model

                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileDescription = request.FileDescription,
                    FileName = request.FileName,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                };

                await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);
            }



            return BadRequest(request);

        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "File Format allowed are jpg,jpeg,png only!!");
            }

            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "Maximum File size allowed is 10Mb !!");
            }
        }
    }
}
