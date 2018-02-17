using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeRentalService.Models;

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
            rent.Buyer = dbContext.Customers.Where(c => c.CustomerId == rent.Buyer.CustomerId).FirstOrDefault();
            rent.RentedBike = dbContext.Bikes.Where(b => b.BikeId == rent.RentedBike.BikeId).FirstOrDefault();
            rent.RentEndTime = null;
            rent.RentStartTime = DateTime.Now;
            rent.TotalCost = 0;
            rent.PaidFlag = false;
            rent.RentalId = 0;

            dbContext.Rentals.Add(rent);

            dbContext.SaveChanges();
            return StatusCode(200, rent);
        }

        [HttpPost]
        [Route("End/{rentalId}")]
        public IActionResult End(int rentalId)
        {
            Rental rent = dbContext.Rentals.Where(r => r.RentalId == rentalId).FirstOrDefault(); //TODO Add Bike to selection

            rent.RentEndTime = DateTime.Now;
            rent.TotalCost = CalculateTotalCost(rent);


            dbContext.SaveChanges();

            return StatusCode(200, rent);
        }

        public double CalculateTotalCost(Rental rent)
        {
            TimeSpan timeDiff = ((DateTime)rent.RentEndTime - (DateTime)rent.RentStartTime);
            var totalMinutes = timeDiff.TotalMinutes;
            var startedHours = (int)Math.Ceiling(((double)totalMinutes) / 60);
            
            return totalMinutes<=15?
                0:
                rent.RentedBike.RentalPriceFirstHour+(rent.RentedBike.RentalPriceExtraHour*(startedHours-1));
        }


        [HttpPost]
        [Route("Paid/{rentalId}")]
        public IActionResult Paid(int rentalId)
        {
            Rental rent = dbContext.Rentals.Where(r => r.RentalId == rentalId).FirstOrDefault();

            rent.PaidFlag = true;

            dbContext.SaveChanges();

            return StatusCode(200, rent);
        }

        [HttpGet]
        [Route("UnpaidRentals")]
        public IActionResult UnpaidRentals(int rentalId) 
            => StatusCode(200, dbContext.Rentals
                .Where(r => r.PaidFlag == false && r.RentEndTime != null && r.TotalCost > 0)
                .SelectMany(r => new object[] { r.Buyer.CustomerId, r.Buyer.FirstName, r.Buyer.LastName, r.RentalId, r.RentStartTime, r.RentEndTime, r.TotalCost }));
    }
}
