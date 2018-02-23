using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeRentalService.Models;
using BikeRentalManager.Controllers;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalService.Controllers
{
    [Route("api/[controller]")]
    public class RentalsController : Controller
    {
        private BikeRentalContext dbContext = new BikeRentalContext();

        [HttpPost]
        [Route("Start")]
        public IActionResult Start([FromBody]Rental rent)
        {
            // Check if a importand parameter is missing
            if (rent.Buyer == null || rent.Buyer.CustomerId == 0 || rent.RentedBike == null || rent.RentedBike.BikeId == 0)
                return StatusCode(400, "Something is missing (Buyer or Bike?)");
            
            // Check if the Customer already has an open rental
            if(dbContext.Rentals.Where(r=>r.Buyer.CustomerId==rent.Buyer.CustomerId&&r.RentEndTime==null).ToArray().Length!=0)
                return StatusCode(400, "This Customer already has an active rental");

            // Set the Buyer and Bike as a Reference and reset other parameters
            rent.Buyer = dbContext.Customers.Where(c => c.CustomerId == rent.Buyer.CustomerId).FirstOrDefault();
            if(rent.Buyer==null)
                return StatusCode(400, "Cant find Buyer");

            rent.RentedBike = dbContext.Bikes.Where(b => b.BikeId == rent.RentedBike.BikeId).FirstOrDefault();
            if (rent.RentedBike == null)
                return StatusCode(400, "Cant find Bike");

            rent.RentEndTime = null;
            rent.RentStartTime = DateTime.Now;
            rent.TotalCost = 0;
            rent.PaidFlag = false;
            rent.RentalId = 0;

            // Add the rental to the databse
            dbContext.Rentals.Add(rent);

            dbContext.SaveChanges();
            return StatusCode(200, rent);
        }

        [HttpPost]
        [Route("End/{rentalId}")]
        public IActionResult End(int rentalId)
        {
            // Get Rental from RentalId
            Rental rent = dbContext.Rentals.Include("RentedBike").Where(r => r.RentalId == rentalId && r.RentedBike != null).FirstOrDefault();

            if (rent == null)
                return StatusCode(400, "Rental does not exist");

            // Get the rented Bike from the RentalId
            Bike rentedBike = dbContext.Rentals.Where(r => r.RentalId == rentalId).Select(r => r.RentedBike).FirstOrDefault();

            // Set the missing parameters to end the rental
            rent.RentEndTime = DateTime.Now;
            rent.TotalCost = RentalFunctions.CalculateTotalCost(rent);
            rentedBike.DateOfLastService = (DateTime)rent.RentEndTime;

            
            dbContext.SaveChanges();

            return StatusCode(200, rent);
        }


        [HttpPost]
        [Route("Paid/{rentalId}")]
        public IActionResult Paid(int rentalId)
        {
            // Get rental from id
            Rental rent = dbContext.Rentals.Where(r => r.RentalId == rentalId).FirstOrDefault();

            // Rental can only be set to paid if it has already ended and the cost is >0
            if (rent.RentEndTime != null && rent.TotalCost != 0)
            {
                rent.PaidFlag = true;

                dbContext.SaveChanges();
                return StatusCode(200, rent);
            }
            return StatusCode(400, "Cant pay this Rental (Not ended or Cost is 0)");
        }

        [HttpGet]
        [Route("UnpaidRentals")]
        public IActionResult UnpaidRentals(int rentalId) 
            => StatusCode(200, dbContext.Rentals
                .Where(r => r.PaidFlag == false && r.RentEndTime != null && r.TotalCost > 0)
                .SelectMany(r => new object[] { r.Buyer.CustomerId, r.Buyer.FirstName, r.Buyer.LastName, r.RentalId, r.RentStartTime, r.RentEndTime, r.TotalCost }));
    }
}
