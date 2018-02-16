using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalService.Models
{
    public class BikeRentalContext : DbContext
    {
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PersonGender> PersonGenders { get; set; }
        public DbSet<BikeCategory> BikeCategorys { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BikeRental;Trusted_Connection=True");
        }
    }
}
