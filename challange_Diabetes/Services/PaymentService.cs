using Microsoft.Extensions.Configuration;
using Stripe;
using System.Threading.Tasks;

namespace challenge_Diabetes.Services
{
    public class PaymentService
    {
        private readonly string _secretKey;

        public PaymentService(IConfiguration configuration)
        {
            _secretKey = configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = _secretKey;
        }

        public async Task<Charge> ProcessPayment(string Email, int amount, string currency)
        {
            var options = new ChargeCreateOptions
            {
                Amount = amount,
                Currency = currency,
                Description = "Sample Charge",
                Source = Email,
            };
            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);
            return charge;
        }
    }
}
