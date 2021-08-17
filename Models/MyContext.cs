using Microsoft.EntityFrameworkCore;

namespace DonationManagement.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        // the "Users" table name will come from the DbSet variable name
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}