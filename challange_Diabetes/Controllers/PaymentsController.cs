using challange_Diabetes.Model;
using challange_Diabetes.Services;
using challenge_Diabetes.Migrations;
using challenge_Diabetes.Model;
using challenge_Diabetes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace challenge_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("charge")]
        public async Task<IActionResult> Charge([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            try
            {
                 var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                request.user_Id= userid;
                var charge = await _paymentService.ProcessPayment(request.Token, request.Amount, request.Currency);

                if (charge.Status == "succeeded")
                {
                    return Ok(charge);
                }
                return BadRequest(charge);
            }
            catch (StripeException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
           
        }
    }
}
