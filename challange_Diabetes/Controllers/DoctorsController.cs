using challange_Diabetes.DTO;
using challange_Diabetes.Model;
using challange_Diabetes.Services;
using challenge_Diabetes.Data;
using challenge_Diabetes.Migrations;
using challenge_Diabetes.Model;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace challenge_Diabetes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CloudinaryService _cloudinaryService;
        

        public DoctorsController( ApplicationDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
          
        }
        [HttpPost("AddDoctor")]
        [Authorize(Roles ="Follower")]

        public async Task<IActionResult> Add( [FromForm]DoctorDTO doctor)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var uploadResult = await _cloudinaryService.UploadImageAsync(doctor.Photo);
            if (uploadResult.Error != null)
            {
                return BadRequest(new { error = uploadResult.Error.Message });

            }


            var Doctor = new Doctor
            {
                userName=doctor.UserName,
                phone=doctor.Phone,
                Photo = uploadResult.SecureUrl.ToString(),
                Email =doctor.Email,
                appointment=doctor.Appointment,
                address=doctor.Address,
                DetectionPrice=doctor.DetectionPrice,
                Doctorspecialization = doctor.DoctorSpecialization,
                Password = doctor.Password,
                about=doctor.about,
                waitingTime=doctor.waitingTime,
              

            };

            _context.Add(Doctor);
            _context.SaveChanges();
            return Ok(new { Message = "Doctor added successfully!" ,Date=Doctor}); ;
        }



        [HttpGet("SelectDoctors")]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _context.Doctors
          /*.Select(d => new DoctorDTO
          {
              UserName = d.userName,
              Phone = d.phone,
              Email = d.Email,
              Address = d.address,
              Appointment = d.appointment,
              DetectionPrice = d.DetectionPrice,
              DoctorSpecialization = d.Doctorspecialization,
             
          })*/
          .ToListAsync();

            return Ok(doctors);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var doctor = await _context.Doctors
          .Where(d => d.Id == id)
         /* .Select(d => new DoctorDTO
          {
              UserName = d.userName,
              Phone = d.phone,
              Email = d.Email,
              Address = d.address,
              Appointment = d.appointment,
              DetectionPrice = d.DetectionPrice,
              DoctorSpecialization = d.Doctorspecialization,
              Photo = d.Photo != null ? Convert.ToBase64String(d.Photo) : null
          })*/
          .FirstOrDefaultAsync();

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);











            /*var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
           
            return Ok(doctor);
        }*/
        }
        [HttpPost("Reservation")]
        public async Task<IActionResult> patientReservationAsync( int id, ReservationDTO reserve){

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound(new { Message = "Doctor not found" });
            }
            var existingReservation = await _context.Reservations
               .Where(r => r.doctor_Id == doctor.Id && r.Date == reserve.Date && r.Time == reserve.Time)
                 .FirstOrDefaultAsync();

            if (existingReservation != null)
            {
                return BadRequest(new { Message = "غير متاح هذا الوقت " });
            }


            var patient = new Reservation
            {
                Username = reserve.Username,
                Phone = reserve.Phone,
                age = reserve.age,
                sex = reserve.sex,
                Date=reserve.Date,
                Time=reserve.Time
                
            };
            patient.doctor_Id = doctor.Id;
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
           

            patient.user_Id = userid;
            _context.Add(patient);
            _context.SaveChanges();
            return Ok(new { Message = "patient success in reservation ", Data = patient });
            

        }

       
       

    }
    }

