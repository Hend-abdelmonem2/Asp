using challange_Diabetes.DTO;
using challange_Diabetes.Services;
using challenge_Diabetes.Data;
using challenge_Diabetes.Model;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace challange_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    { private readonly ApplicationDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public SportsController(ApplicationDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }
        [HttpPost]
        [Authorize(Roles = "Follower")]
        public async Task<IActionResult>Add([FromForm]sportDTO sport)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var uploadResult = await _cloudinaryService.UploadImageAsync(sport.Image);
            if (uploadResult.Error != null)
            {
                return BadRequest(new { error = uploadResult.Error.Message });

            }
            var sports = new Sport
            {
                Name = sport.Name,
                Description = sport.Description,
                Photo = uploadResult.SecureUrl.ToString()
            };
            _context.Add(sports);
            _context.SaveChanges();
            return Ok(new { Message = "sport added successfully!", Date = sports });


        }
        [HttpGet("DisplaySports")]
        public IActionResult Get()
        {
            var sports = _context.sports.Select(x => new
            {
                x.Name,
                x.Description,
                x.Photo
            })
            .ToList();
            return  Ok (sports);

        }
    }
}
