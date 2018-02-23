using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeRentalService.Models;

namespace BikeRentalService.Controllers
{
    [Route("api/[controller]")]
    public class BikesController : Controller
    {
        private BikeRentalContext dbContext = new BikeRentalContext();


        [HttpGet]
        public IActionResult GetAllBikes()
            => StatusCode(200, dbContext.Bikes.Select(e => e).ToList());

        [HttpGet]
        [Route("Available")]
        public IActionResult GetAllRentals(string sort)
            => StatusCode(200, dbContext.Bikes.Select(e => e)
                .Where(b =>
                    //Select all Rentals of this bike where the RentEndTime is null
                    dbContext.Rentals.Where(r => r.RentEndTime == null && r.RentedBike.BikeId == b.BikeId)
                    //Check if the List of the active orders whith this bike is length 0
                    .Select(r => r).ToArray().Length == 0)
                .OrderBy(b => 
                    sort=="pofh"?
                        b.RentalPriceFirstHour
                        : sort=="poah" ? 
                            b.RentalPriceExtraHour
                            : b.BikeId
                )
                .OrderByDescending(b=>
                    sort == "pd" ?
                        b.PurchaseDate.Ticks
                        : b.BikeId
                ));


        [HttpPost]
        [Route("Update/{bikeId}")]
        public IActionResult Update(int bikeId, [FromBody]Bike bike)
        {
            Bike updateElement = dbContext.Bikes.Select(e => e).Where(e => e.BikeId == bikeId).FirstOrDefault();

            updateElement.Brand = bike.Brand == null ? updateElement.Brand : bike.Brand;
            updateElement.Category = bike.Category == null ? updateElement.Category : dbContext.BikeCategorys.Where(c => c.CategoryId == bike.Category.CategoryId).FirstOrDefault();
            updateElement.DateOfLastService = bike.DateOfLastService == null ? updateElement.DateOfLastService : bike.DateOfLastService;
            updateElement.Notes = bike.Notes == null ? updateElement.Notes : bike.Notes;
            updateElement.PurchaseDate = bike.PurchaseDate == null ? updateElement.PurchaseDate : bike.PurchaseDate;
            updateElement.RentalPriceExtraHour = bike.RentalPriceExtraHour == 0 ? updateElement.RentalPriceExtraHour : bike.RentalPriceExtraHour;
            updateElement.RentalPriceFirstHour = bike.RentalPriceFirstHour == 0 ? updateElement.RentalPriceFirstHour : bike.RentalPriceFirstHour;

            dbContext.SaveChanges();
            return StatusCode(200, updateElement);
        }

        [HttpPut]
        [Route("Create")]
        public IActionResult Create([FromBody]Bike bike)
        {
            bike.Category = dbContext.BikeCategorys.Where(c => c.CategoryId == bike.Category.CategoryId).FirstOrDefault();
            dbContext.Bikes.Add(bike);
            dbContext.SaveChanges();
            return StatusCode(200, bike);
        }
        
        [HttpDelete]
        [Route("Delete/{bikeId}")]
        public IActionResult Delete(int bikeId)
        {
            Bike deletingBike = dbContext.Bikes.Where(b => b.BikeId == bikeId).FirstOrDefault();

            if (deletingBike == null) return StatusCode(400, "Delete Failed");
            if(dbContext.Rentals.Where(r=>r.RentedBike.BikeId==bikeId).ToArray().Length>0) return StatusCode(400, "Delete Failed");

            dbContext.Bikes.Remove(deletingBike);
            dbContext.SaveChanges();
            return StatusCode(200, "Delete Successful");

        }
    }
}
