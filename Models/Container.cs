using System;
using System.Collections.Generic;

namespace DonationManagement.Models
{
    public class Container
    {
        public Admin Admin { get; set; }
        public Donor Donor { get; set; }
        public List<Donor> Donors { get; set; }
        public Patient Patient { get; set; }
        public List<Patient> Patients { get; set; }
    }
}