using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagement.Models
{
    public class Donor
    {
        [Key]
        [Required]
        public int DonorId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime Birthday { get; set; }

        [Required]
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [Required]
        [Display(Name="Gender: ")]
        public string Gender { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]  
        public long PhoneNumber { get; set; }

        [Required]
        public string Hospital { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // [Required]
        // [DataType(DataType.Password)]
        // [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
        // public string Password { get; set; }

        // [NotMapped]
        // [Required]
        // [DataType(DataType.Password)]
        // [MinLength(8, ErrorMessage = "Confirm must be 8 characters or longer!")]
        // [Compare("Password")]
        // [Display(Name = "Confirm Password")]
        // public string Confirm { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // public int AdminId { get; set; }
        // public Admin AccpetedBy { get; set; }
        public List<Donation> Donate { get; set; }

    }
}