using challange_Diabetes.DTO;
using challange_Diabetes.Model;
using challenge_Diabetes.Data;
using challenge_Diabetes.Migrations;
using challenge_Diabetes.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace challange_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("AddFavoriteDoctor")]
        public async Task<IActionResult> AddToFavorites([FromBody] AddToFavoritesDTO favorite)
        {
             var userid= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
             if (userid == null)
            {
                return BadRequest();
            }
             
            var favorites = new Favorite
            {
                DoctorId = favorite.DoctorId
            };
            favorites.UserId = userid;
            _context.Favorites.Add(favorites);
            await _context.SaveChangesAsync();
            return Ok(favorites);
        }
        [HttpDelete("DeleteFavoriteDoctor")]
        public async Task<IActionResult> RemoveFromFavorites(RemoveFromFavoritesDTO favorite)
        {
            // Validate the model
            if (favorite == null || favorite.DoctorId == null)
            {
                return BadRequest("Invalid favorite data.");
            }

            // Retrieve the user ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing.");
            }

            // Find the favorite entry by DoctorId and UserId
            var favoriteEntry = await _context.Favorites
                                              .FirstOrDefaultAsync(f => f.DoctorId == favorite.DoctorId && f.UserId == userId);

            if (favoriteEntry == null)
            {
                return NotFound("Favorite entry not found.");
            }

            // Remove the favorite entry
            _context.Favorites.Remove(favoriteEntry);
            await _context.SaveChangesAsync();
            return Ok("Removed successfully");
        }
            [HttpGet("GetFavoritesDoctors")]
        public async Task<IActionResult> GetFavorites()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var favorites = await _context.Favorites
                .Where(f => f.UserId == userid)
                .Include(f => f.Doctor)
                 .ToListAsync();
            var favoriteDoctors = favorites.Select(f => new
            {
                
                Doctor = new
                {   f.Doctor.Id,
                    f.Doctor.userName,
                    f.Doctor.Doctorspecialization,
                    f.Doctor.about,
                    f.Doctor.address,
                    f.Doctor.Photo
                }
            });

            return Ok(favoriteDoctors);
        }

    }
}
