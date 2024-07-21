namespace challenge_Diabetes.Model
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public bool IsDefault { get; set; }
        public string user_Id { get; set; }

    }
}
