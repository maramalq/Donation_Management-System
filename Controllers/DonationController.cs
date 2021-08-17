using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DonationManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace DonationManagement.Controllers
{
    public class DonationController : Controller
    {
        private MyContext _db;
        private int? uid
        {
            get { return HttpContext.Session.GetInt32("AdminId"); }
        }
        private bool isLoggedIn
        {
            get { return uid != null; }
        }
        public DonationController(MyContext context)
        {
            _db = context;
        }

        [HttpGet("/donor")]
        public IActionResult Donor()
        {
            return View();
        }

        [HttpPost("create_donor")]
        public IActionResult CreateDonor(Donor donor)
        {
            if(ModelState.IsValid)
            {
                _db.Donors.Add(donor);
                _db.SaveChanges();
                Console.WriteLine("Successfully added");
                return RedirectToAction("Success" , "Donation");

            }
            Console.WriteLine("Errrrrrors");
            return View("Donor");
        }

        [HttpGet("patient")]
        public IActionResult Patient()
        {
            return View();
        }

        [HttpPost("create_patient")]
        public IActionResult CreatePatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                // If a User exists with provided email
                if ( _db.Patients.Any(p => p.Email == patient.Email) )
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    // You may consider returning to the View at this point
                    return View("Patient");
                }
                // if we reach here it confirms this is new user
                // add them to db... after we hash the password
                PasswordHasher<Patient> Hasher = new PasswordHasher<Patient>();
                patient.Password = Hasher.HashPassword(patient, patient.Password);
                // after hashing password add the user to db
                _db.Patients.Add(patient);
                _db.SaveChanges();
                // save their id to session
                HttpContext.Session.SetInt32("PatientId", patient.PatientId);
                // redirect to Dashboard
                //return RedirectToAction("Success");
                return Redirect($"/info/{patient.PatientId}");
            }
            // other code
            return View("Patient");
        }

        [HttpPost("patient_login")]
        public IActionResult PatientLogin(LoginPatient patient)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var patientInDb = _db.Patients.FirstOrDefault(p => p.Email == patient.LoginEmail);
                // If no user exists with provided email
                if (patientInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    Console.WriteLine("email error");
                    return View("Patient");
                }
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginPatient>();
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(patient, patientInDb.Password, patient.LoginPassword);
                // result can be compared to 0 for failure
                if (result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                    Console.WriteLine("password error");
                    return View("Patient");
                }
                // if result is not 0, then it is valid
                // Store user id into session
                HttpContext.Session.SetInt32("PatientId", patientInDb.PatientId);
                return Redirect($"/info/{patientInDb.PatientId}");
            }
            Console.WriteLine("errrrorrrrs");
            return View("Patient");
        }

        [HttpGet("info/{patientId}")]
        public IActionResult Info(int patientId)
        {
            Patient thisPatient = _db.Patients
                .Include(p => p.Needer)
                .ThenInclude(p => p.Donor)
                .FirstOrDefault(p => p.PatientId == patientId);

            return View(thisPatient);
        }

        [HttpGet("edit/{patientId}")]
        public IActionResult EditPatient(int patientId)
        {
            Patient thisPatient = _db.Patients
                .Include(p => p.Needer)
                .ThenInclude(p => p.Donor)
                .FirstOrDefault(p => p.PatientId == patientId);

            return View(thisPatient);
        }

        [HttpPost("update/{patientId}")]
        public IActionResult UpdatePatient(int patientId , Patient patient)
        {
            if(ModelState.IsValid)
            {
                Patient patientDB = _db.Patients.FirstOrDefault(p => p.PatientId == patientId);

                patientDB.Name = patient.Name;
                patientDB.Birthday = patient.Birthday;
                patientDB.BloodGroup = patient.BloodGroup;
                patientDB.Gender = patient.Gender;
                patientDB.Address = patient.Address;
                patientDB.PhoneNumber = patient.PhoneNumber;
                patientDB.Email = patient.Email;
                patientDB.Hospital = patient.Hospital;

                _db.SaveChanges();
                Console.WriteLine("successfully updated");
                return Redirect($"/info/{patient.PatientId}");
            }

            Patient thisPatient = _db.Patients
                .Include(p => p.Needer)
                .ThenInclude(p => p.Donor)
                .FirstOrDefault(p => p.PatientId == patientId);

            Console.WriteLine("There were some errors, should see errors");
            return View("EditPatient");
        }

        [HttpGet("successs")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("dashboard/donors")]
        public IActionResult Dashboard()
        {
            if( !isLoggedIn )
            {
                return RedirectToAction("Index" , "Home"); 
            }
            Container container = new Container();

            List<Donor> allDonors = _db.Donors
                .Include(d => d.Donate)
                .ThenInclude(d => d.Patient)
                .ToList();
            List<Patient> allPatients = _db.Patients
                .Include(p => p.Needer)
                .ThenInclude(p => p.Donor)
                .ToList();
            Admin admin = _db.Admins.FirstOrDefault(a => a.AdminId == (int)uid);

            container.Donors = allDonors;
            container.Patients = allPatients;
            container.Admin = admin;

            return View(container);
        }

        [HttpGet("dashboard/patients")]
        public IActionResult PatientsList()
        {
            if( !isLoggedIn )
            {
                return RedirectToAction("Index" , "Home"); 
            }
            Container container = new Container();

            List<Donor> allDonors = _db.Donors
                .Include(d => d.Donate)
                .ThenInclude(d => d.Patient)
                .ToList();
            List<Patient> allPatients = _db.Patients
                .Include(p => p.Needer)
                .ThenInclude(p => p.Donor)
                .ToList();
            Admin admin = _db.Admins.FirstOrDefault(a => a.AdminId == (int)uid);

            container.Donors = allDonors;
            container.Patients = allPatients;
            container.Admin = admin;

            return View(container);

        }


        [HttpGet("response/{donorId}")]
        public IActionResult ResponsePage(int donorId)
        {
            if( !isLoggedIn )
            {
                return RedirectToAction("Index" , "Home"); 
            }
            Container container = new Container();

            Donor thisDonor = _db.Donors
                .Include(d => d.Donate)
                .ThenInclude(d => d.Patient)
                .FirstOrDefault(d => d.DonorId == donorId);

            List<Patient> AllPatients = _db.Patients.ToList();

            container.Donor = thisDonor;
            container.Patients = AllPatients;
            
            return View(container);
        }

        [HttpPost("process_response/{donorId}")]
        public IActionResult ProcessResponse(int AssoId , int donorId)
        {
            // create new donation
            Donation donation = new Donation();
            // assign the id for the new donation
            donation.PatientId = AssoId;
            donation.DonorId = donorId;
            // add it to the donations table
            _db.Donations.Add(donation);
            _db.SaveChanges();

            Container container = new Container();

            List<Donor> allDonors = _db.Donors.ToList();
            List<Patient> allPatients = _db.Patients.ToList();
            Admin admin = _db.Admins.FirstOrDefault(a => a.AdminId == (int)uid);

            container.Donors = allDonors;
            container.Patients = allPatients;
            container.Admin = admin;

            return RedirectToAction("Dashboard", container);
        }

        [HttpGet("send_email")]
        public IActionResult SendEmail()
        {
            if( !isLoggedIn )
            {
                return RedirectToAction("Index" , "Home"); 
            }
            return View();
        }

        [HttpPost("sending_email")]  
        public ActionResult SendingEmail(string receiver, string subject, string message) {  
            try {  
                if (ModelState.IsValid) {  
                    var senderEmail = new MailAddress("spoiled.coders@gmail.com", "Spoiled Coders Team");  
                    var receiverEmail = new MailAddress(receiver, "Receiver");  
                    var password = "coders2021";  
                    var sub = subject;  
                    var body = message;  
                    var smtp = new SmtpClient {  
                        Host = "smtp.gmail.com",  
                            Port = 587,  
                            EnableSsl = true,  
                            DeliveryMethod = SmtpDeliveryMethod.Network,  
                            UseDefaultCredentials = false,  
                            Credentials = new NetworkCredential(senderEmail.Address, password)  
                    };  
                    using(var mess = new MailMessage(senderEmail, receiverEmail) {  
                        Subject = subject,  
                            Body = body  
                    }) {  
                        smtp.Send(mess);  
                    }  
                    return RedirectToAction("Dashboard");  
                }  
            } catch (Exception) {  
                ViewBag.Error = "Some Error";  
            }  
            return View("SendEmail");  
        }

        [HttpGet("delete_donor/{donorId}")]
        public ActionResult DeleteDonor(int donorId)
        {
            if( !isLoggedIn )
            {
                return RedirectToAction("Index" , "Home"); 
            }
            Donor deleted = _db.Donors.FirstOrDefault(d => d.DonorId == donorId);
            _db.Donors.Remove(deleted);
            // save changes
            _db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("delete_patient/{patientId}")]
        public ActionResult DeletePatient(int patientId)
        {
            if( !isLoggedIn )
            {
                return RedirectToAction("Index" , "Home"); 
            }
            Patient deleted = _db.Patients.FirstOrDefault(p => p.PatientId == patientId);
            _db.Patients.Remove(deleted);
            // save changes
            _db.SaveChanges();
            return RedirectToAction("Dashboard");
        }    
     
    }
}