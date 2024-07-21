using challenge_Diabetes.DTO;
using challenge_Diabetes.Data;
using challenge_Diabetes.DTO;
using challenge_Diabetes.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace challenge_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public MeasurementsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost("blood sugar level")]
        public async Task<IActionResult> blood_sugar( Sugar_ReadingDTO reading)
        {
            if (ModelState.IsValid)

            {
                var user = new Measuring_Sugar();
                user.sugar_reading = reading.sugar_reading;
                user.measurement_date = reading.measurement_date;
                user.DateTime = DateTime.Now;




                var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                user.User_Id = userid;
                _dbContext.Add(user);
                _dbContext.SaveChanges();

                if (user.sugar_reading > 180)
                  {
                    return Ok(new { Message = "معدل السكر مرتفع يجب قياس السكر مره اخري بعد ساعتين ثم الفحص", Data = user }); 

                  }
   
                if (user.sugar_reading < 80)
                {
                    return Ok(new { Message = "معل السكر منخفض يجب قياسه بعد ساعتين ثم المتابعه والفحص", Data = user }); 
                }
            return Created("/api/Measurements/" + userid, new { Message = "created", Data = user });

            }
            else
            {
                return BadRequest(ModelState);
            }
            
        }
        [HttpPost("blood pressure")]
        public async Task<IActionResult>blood_pressure(blood_pressureDTO pressure)
        {
            if (ModelState.IsValid)
            {
                Measuring_pressure add = new Measuring_pressure();
                add.Diastolic_pressure = pressure.Diastolic_pressure;
                add.Systolic_pressure = pressure.Systolic_pressure;
                add.Heart_rate = pressure.Heart_rate;
                add.DateTime = DateTime.Now;
                var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                add.User_Id = userid;
                _dbContext.Add(add);
                _dbContext.SaveChanges();
                if (add.Diastolic_pressure > 130 || add.Systolic_pressure < 80)
                {

                    return Ok(new { Message = "ضغط الدم غير طبيعي يجب المتابعه و الفحص", Data = add });
                }
                if (add.Diastolic_pressure < 120 || add.Systolic_pressure > 80)
                {
                    return Ok(new { Message = "ضغط الدم غير طبيعي يجب المتابعه و الفحص", Data = add });
                }
                if (add.Heart_rate > 100 || add.Heart_rate < 60)
                {
                    return Ok(new { Message = "معدل ضربات القلب غير طبيعي يجب الفحص ", Data = add });
                }
                                
             return Created("/api/Measurements/" + userid, new { Message = "created", Data = add });

            }
            return BadRequest(ModelState);
        }
        [HttpPost("weight")]
        public async Task <IActionResult>weight_measurement(WeightDTO weight)
        {
            if (ModelState.IsValid)
            {
                Measuring_weight Weight = new Measuring_weight();
                Weight.weight = weight.weight;
                Weight.sport= weight.sport;
                Weight.DateTime = DateTime.Now;
                var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                Weight.User_Id = userid;
                _dbContext.Add(Weight);
                _dbContext.SaveChanges();
                return Created("/api/Measurements/" + userid, new { Message = "created", Data = Weight });
            }
            return BadRequest(ModelState);
        }
    }
}
   
