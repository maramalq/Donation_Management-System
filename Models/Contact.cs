using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagement.Models
{
    public class Contact
    {
        [Key]
        [Required]
        public int ContactId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Display(Name="Message")]
        public string Msg { get; set; }
    }
}