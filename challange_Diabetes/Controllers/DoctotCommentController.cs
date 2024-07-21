using challange_Diabetes.Model;
using challenge_Diabetes.Data;
using challenge_Diabetes.Migrations;
using challenge_Diabetes.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DoctorComment = challange_Diabetes.Model.DoctorComment;

namespace challange_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctotCommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctotCommentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        [HttpPost("AddDoctorcomment")]
        
        public async Task<ActionResult<PostComment>> CreateComment(int DoctorId, Model.DoctorComment comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
             var user= await  _userManager.FindByIdAsync(userId);
            
           if (user == null)
            {
                return NotFound();
            }
           
           
            var Doctor = await _context.Doctors.FindAsync(DoctorId);
            if (Doctor == null)
            {
                return NotFound();
            }
            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;
            comment.DoctorId = DoctorId;
            comment.UserName = user.UserName;
            comment.UserProfilePictureUrl = user.Photo;
            _context.DoctorComments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }
        [HttpGet("GetDoctorcomment")]
        public async Task<ActionResult<DoctorComment>> GetComment(int id)
        {
            var comment = await _context.DoctorComments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }
        [HttpGet("Get comments")]
        public async Task <IActionResult>Getcomments(int doctorId)
        {
          var   doctor =await _context.Doctors.FindAsync(doctorId);
            if(doctor == null)
            {
                return NotFound("Doctor  not found");
            }
             var Comments = _context.DoctorComments.Where(X=>X.DoctorId == doctorId).ToList();
             return Ok(Comments);


        }
        [HttpPut("UpdateDoctorcomment")]
        
        public async Task<IActionResult> UpdateComment(int id, DoctorComment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            var existingComment = await _context.DoctorComments.FindAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || userId != existingComment.UserId)
            {
                return Forbid();
            }

            existingComment.Content = comment.Content;
            _context.Entry(existingComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingComment);
        }
        [HttpDelete("DeleteDoctorcomment")]
        
        public async Task<IActionResult> DeleteComment([FromBody] int id)
        {
            var comment = await _context.DoctorComments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || userId != comment.UserId)
            {
                return Forbid();
            }

            _context.DoctorComments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }
        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
