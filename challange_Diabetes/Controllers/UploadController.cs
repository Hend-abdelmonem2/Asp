using challange_Diabetes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace challange_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;

        public UploadController(CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            var uploadResult = await _cloudinaryService.UploadImageAsync(file);

            if (uploadResult.Error != null)
            {
                return BadRequest(new { error = uploadResult.Error.Message });
            }

            return Ok(new
            {
                publicId = uploadResult.PublicId,
                url = uploadResult.SecureUrl
            });
        }
    }
}
