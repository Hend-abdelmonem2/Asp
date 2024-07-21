namespace challenge_Diabetes.Model
{
    public class PaymentRequest
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public int Amount { get; set; } // Amount in cents
        public string Currency { get; set; }
        public string user_Id { get; set; } 

    }
}
