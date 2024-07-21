using challange_Diabetes.DTO;
using challange_Diabetes.Model;
using challenge_Diabetes.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace challenge_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }
        private static List<Post> posts = new List<Post>();

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            var posts = await _context.Posts.Include(p=>p.Likes).Include(p=>p.Comments)
           .ToListAsync();
            return Ok(posts);
            //return posts;
        }

        // GET: api/Posts/5
        [HttpGet("Getpost")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.Include(p => p.Comments ).Include(p=>p.Likes).FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // POST: api/Posts
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> CreatePost(postDTO post)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var Post = new Post();
            Post. UserId = userId;
            Post.CreatedAt = DateTime.UtcNow;
            Post.Content=post.Content;
            _context.Posts.Add(Post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = Post.Id }, post);
        }

        // PUT: api/Posts/5
        [HttpPut("Updatepost")]
        [Authorize]
        public async Task<IActionResult> UpdatePost( int id,  PostUpdateDTO postUpdateDto)
        {
            if (postUpdateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || userId != existingPost.UserId)
            {
                return Forbid();
            }

            // Update only the Content property
            existingPost.Content = postUpdateDto.Content;
            _context.Entry(existingPost).Property(p => p.Content).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "update successfuly", Date = existingPost });
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }




        // DELETE: api/Posts/5
        [HttpDelete("Deletepost")]
        [Authorize]
        public async Task<IActionResult> DeletePost([FromBody]int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || userId != post.UserId)
            {
                return Forbid();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok("Delete Successfuly");
        }
        [HttpPost("Like post")]
        [Authorize]
        public async Task<IActionResult> LikePost(int postId)
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

            var like = new Like
            {
                UserId = userId,
                PostId = postId,
                LikedAt = DateTime.UtcNow
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("RemoveLike")]
        [Authorize]
        public async Task<IActionResult> UnlikePost(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            if (like == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet("Getcount")]
        public async Task<ActionResult<int>> GetLikesCount(int postId)
        {
            var post = await _context.Posts.Include(p => p.Likes).FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
            {
                return NotFound();
            }

            return post.Likes.Count();
        }



    }

}

