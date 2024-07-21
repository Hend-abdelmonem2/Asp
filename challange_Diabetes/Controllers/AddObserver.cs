using challenge_Diabetes.Data;
using challenge_Diabetes.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace challenge_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddObserver : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public AddObserver(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost("AddObserver")]
        public IActionResult CreateObserver( string email, string phone, string relvant_relation)
        {

            var observer = new Observer
            {
                Email = email,
                phone = phone,
                relvant_relation = relvant_relation
              
        };
             var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
             observer.User_Id = userid;
            _dbContext.Add(observer);
            _dbContext.SaveChanges();

            return Ok(observer);
        }
        [HttpGet("Get All Observer")]
        public IActionResult GetObserver()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var observers=_dbContext.Observers.Where(y => y.User_Id == userid).Select(m=>new
            {
                User_Id = userid,
                Email = m.Email,
                phone=m.phone,
                relavant_relation=m.relvant_relation
            })
                .ToList();
            return Ok(observers);

        }
    }

}

