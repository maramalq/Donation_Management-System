using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationManagement.Models
{
    public class Donation
    {
        [Key]
        [Required]
        public int DonationId { get; set; }

        public int DonorId { get; set; }
        public Donor Donor { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}