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
using System.Net.Mail;
using System.Net;

namespace DonationManagement.Controllers
{
    public class HomeController : Controller
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
        public HomeController(MyContext context)
        {
            _db = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("admin_register")]
        public IActionResult AdminRegister()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(Admin admin)
        {
            // Check initial ModelState => if there are no errors
            if (ModelState.IsValid)
            {
                // If a User exists with provided email
                if ( _db.Admins.Any(a => a.Email == admin.Email) )
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    // You may consider returning to the View at this point
                    return View("AdminRegister");
                }
                // if we reach here it confirms this is new user
                // add them to db... after we hash the password
                PasswordHasher<Admin> Hasher = new PasswordHasher<Admin>();
                admin.Password = Hasher.HashPassword(admin, admin.Password);
                // after hashing password add the user to db
                _db.Admins.Add(admin);
                _db.SaveChanges();
                // save their id to session
                HttpContext.Session.SetInt32("AdminId", admin.AdminId);
                // redirect to Dashboard
                return RedirectToAction("Dashboard" , "Donation");
            }
            // other code
            return View("AdminRegister");
        }

        [HttpGet("admin_login")]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginAdmin admin)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var adminInDb = _db.Admins.FirstOrDefault(a => a.Email == admin.LoginEmail);
                // If no user exists with provided email
                if (adminInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    Console.WriteLine("email error");
                    return View("AdminLogin");
                }
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginAdmin>();
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(admin, adminInDb.Password, admin.LoginPassword);
                // result can be compared to 0 for failure
                if (result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                    Console.WriteLine("password error");
                    return View("AdminLogin");
                }
                // if result is not 0, then it is valid
                // Store user id into session
                HttpContext.Session.SetInt32("AdminId", adminInDb.AdminId);
                return RedirectToAction("Dashboard" , "Donation");
            }
            Console.WriteLine("errrrorrrrs");
            return View("AdminLogin");
        }

        [HttpGet("about_us")]
        public IActionResult Aboutus(){
            return View();
        }
        
        [HttpGet("contact_us")]
        public IActionResult Contactus()
        {
            return View();
        }
        [HttpPost("contact_proccess")]
        public IActionResult Contact(Contact contact){
            if (ModelState.IsValid)
            {
                // provide userID from session
                // if correct, then create
                // contact.UserId = (int)uid;
                _db.Contacts.Add(contact);
                // save
                _db.SaveChanges();
                // redirect
                 try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(contact.EmailAddress);//Email which you are getting 
								//from contact us page 
                    msz.To.Add("spoiled.coders@gmail.com");//Where mail will be sent 
                    msz.Subject = contact.Title;
                    msz.Body = contact.Msg;
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";

                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
					("spoiled.coders@gmail.com", "coders2021");

                    smtp.EnableSsl = true;

                    smtp.Send(msz);

                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch(Exception ex )
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }      
                Console.WriteLine($"Successfully Saved Message {contact.Title}");
                return View("Contactus");
            }
            
            else {
            Console.WriteLine("You are here ! there is errors !");
            return View("Contactus");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
