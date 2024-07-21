using challange_Diabetes.Model;
using challenge_Diabetes.Data;
using challenge_Diabetes.Migrations;
using challenge_Diabetes.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace challenge_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentMethodsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetPaymentsMethod")]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _context.Users.Include(u => u.PaymentMethods).FirstOrDefaultAsync(u => u.Id == userid);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.PaymentMethods);
        }
        [HttpPost("AddPaymentMethod")]
        public async Task<IActionResult> AddPaymentMethod([FromBody] PaymentMethod paymentMethod)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.Include(u => u.PaymentMethods).FirstOrDefaultAsync(u => u.Id == userid);
            if (user == null)
            {
                return NotFound();
            }

            if (paymentMethod.IsDefault)
            {
                var currentDefault = user.PaymentMethods.FirstOrDefault(pm => pm.IsDefault);
                if (currentDefault != null)
                {
                    currentDefault.IsDefault = false;
                    _context.PaymentMethods.Update(currentDefault);
                }
            }
            paymentMethod.user_Id=user.Id;
            user.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync();

            return Ok(paymentMethod);
        }
        [HttpDelete("DeletePayment")]
        public async Task<IActionResult> DeletePaymentMethod(int paymentMethodId)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _context.Users.Include(u => u.PaymentMethods).FirstOrDefaultAsync(u => u.Id == userid);
            if (user == null)
            {
                return NotFound();
            }

            var paymentMethod = user.PaymentMethods.FirstOrDefault(pm => pm.Id == paymentMethodId);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            user.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();

            return Ok(paymentMethod);


            

        }
        [HttpPut("set-default")]
        public async Task<IActionResult> SetDefaultPaymentMethod(int paymentMethodId)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _context.Users.Include(u => u.PaymentMethods).FirstOrDefaultAsync(u => u.Id == userid);
            if (user == null)
            {
                return NotFound();
            }

            var paymentMethod = user.PaymentMethods.FirstOrDefault(pm => pm.Id == paymentMethodId);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            var currentDefault = user.PaymentMethods.FirstOrDefault(pm => pm.IsDefault);
            if (currentDefault != null)
            {
                currentDefault.IsDefault = false;
                _context.PaymentMethods.Update(currentDefault);
            }

            paymentMethod.IsDefault = true;
            _context.PaymentMethods.Update(paymentMethod);
            await _context.SaveChangesAsync();

            return Ok(paymentMethod);
        }
    }
}
