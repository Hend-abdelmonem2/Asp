using challange_Diabetes.Model;
using challenge_Diabetes.Data;
using challenge_Diabetes.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace challange_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Comments
        [HttpPost("AddPostcomment")]
        [Authorize]
        public async Task<ActionResult<PostComment>> CreateComment( int postId,PostComment comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound();
            }

            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;
            comment.PostId = postId;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }

        // GET: api/Comments/5
        [HttpGet("GetPostcomment")]
        public async Task<ActionResult<PostComment>> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }
        [HttpGet("Get comments")]
        public async Task<IActionResult> Getcomments(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound("post  not found");
            }
            var Comments = _context.Comments.Where(X => X.PostId == postId).ToList();
            return Ok(Comments);


        }

        // PUT: api/Comments/5
        [HttpPut("UpdatePostcomment")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, PostComment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            var existingComment = await _context.Comments.FindAsync(id);
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

        // DELETE: api/Comments/5
        [HttpDelete("DeletePostcomment")]
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromBody] int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || userId != comment.UserId)
            {
                return Forbid();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }

}

