namespace challange_Diabetes.DTO
{
    public class DoctorDTO
    {
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Appointment { get; set; }
        public int  DetectionPrice { get; set; }
        public string DoctorSpecialization { get; set; }
        public string Password { get; set; }
        public IFormFile Photo { get; set; }
        public string about { get; set; }
        public string waitingTime { get; set; }
        

    }
}
