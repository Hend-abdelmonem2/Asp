using challange_Diabetes.DTO;
using challenge_Diabetes.Data;
using challenge_Diabetes.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace challange_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class profilesController : ControllerBase
    { private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public profilesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        

        [HttpGet("Get reservation for user")]
        public IActionResult GetReservations()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var reservations = _context.Reservations.Where(y => y.user_Id == userid).Select(m => new
            {
                username = m.Username,
                phone = m.Phone,
                age = m.age,
                sex = m.sex,
                Date = m.Date
            })
            .ToList();

            return Ok(reservations);
        }
        [HttpDelete("DeleteResrvation")]
        public async Task<IActionResult> DeleteReservation(int id)
        {

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return Ok(reservation);
        }
       
               
        
        }
}
