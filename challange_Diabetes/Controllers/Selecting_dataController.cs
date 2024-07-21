using challenge_Diabetes.Data;
using challenge_Diabetes.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace challenge_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Selecting_dataController : ControllerBase
    { private readonly ApplicationDbContext _context;
       
        public Selecting_dataController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        [HttpGet("user's sugar_data")]
        public async Task<IActionResult> sugar_data(DateTime specificDate)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var  sugardata = _context.measuring_Sugars.Where(x => x.User_Id == userid && x.DateTime.Date == specificDate.Date).Select(x => new
            {   x.User_Id,
                x.DateTime,
                x.measurement_date,
                x.sugar_reading,
                status=x.sugar_reading>180?"مرتفع": x.sugar_reading < 80 ? "منخفض" : " مضبوط",
                
            }).ToList();
            return Ok(sugardata);
        }
        [HttpGet("user's presure_data")]

        public async Task< IActionResult> pressure_data(DateTime specificDate)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var pressuredata = _context.measuring_Pressures.Where(y => y.User_Id == userid && y.DateTime.Date == specificDate.Date).Select(y => new
            {
                y.User_Id,
                y.DateTime,
                y.Systolic_pressure,
                y.Diastolic_pressure,
                y.Heart_rate,
                status=y.Diastolic_pressure>130?"مرتفع":y.Systolic_pressure>80?"مرتفع":y.Diastolic_pressure<120?"منخفض":y.Systolic_pressure<80?"منخفض":"مضبوط",
            }).ToList();
            
            return Ok(pressuredata);
        }

        [HttpGet("user's weight_data")]
         
        public async Task<IActionResult> weight_data(DateTime specificDate)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var weightdata = _context.measuring_Weights.Where(w => w.User_Id == userid && w.DateTime.Date == specificDate.Date).Select(w=>new
            {  w.User_Id,
               w.weight,
               w.sport,
               w.DateTime,

            }).ToList();

            return Ok(weightdata);
        }


    }
}
