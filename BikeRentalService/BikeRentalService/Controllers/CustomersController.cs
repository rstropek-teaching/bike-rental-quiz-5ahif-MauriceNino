using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeRentalService.Models;

namespace BikeRentalService.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private BikeRentalContext dbContext = new BikeRentalContext();


        [HttpGet]
        public IActionResult GetAllCustomers(string lastName) 
            => StatusCode(200, dbContext.Customers.Select(e => e)
                .Where(e => (lastName == null ? e.LastName : lastName) == e.LastName).ToList());

        [HttpGet]
        [Route("GetAllRentals/{custId}")]
        public IActionResult GetAllRentals(int custId)
            => StatusCode(200, dbContext.Rentals.Select(e => e)
                .Where(e => e.Buyer.CustomerId == custId).ToList());


        [HttpPost]
        [Route("Update/{custId}")]
        public IActionResult Update(int custId, [FromBody]Customer cust)
        {
            Customer updateElement = dbContext.Customers.Select(e => e).Where(e => e.CustomerId == custId).FirstOrDefault();
            updateElement.Birthday = cust.Birthday==null?updateElement.Birthday:cust.Birthday;
            updateElement.FirstName = cust.FirstName == null ? updateElement.FirstName : cust.FirstName;
            updateElement.LastName = cust.LastName == null ? updateElement.LastName : cust.LastName;
            updateElement.Gender = cust.Gender==null?updateElement.Gender:dbContext.PersonGenders.Where(e=>e.GenderId==cust.Gender.GenderId).FirstOrDefault();
            updateElement.HouseNumber = cust.HouseNumber == 0 ? updateElement.HouseNumber : cust.HouseNumber;
            updateElement.Street = cust.Street == null ? updateElement.Street : cust.Street;
            updateElement.Town = cust.Town == null ? updateElement.Town : cust.Town;
            updateElement.ZipCode = cust.ZipCode == 0 ? updateElement.ZipCode : cust.ZipCode;

            dbContext.SaveChanges();
            return StatusCode(200, updateElement);
        }

        [HttpPut]
        [Route("Create")]
        public IActionResult Create([FromBody]Customer cust)
        {
            cust.Gender = dbContext.PersonGenders.Where(e => e.GenderId == cust.Gender.GenderId).FirstOrDefault();
            dbContext.Customers.Add(cust);
            dbContext.SaveChanges();
            return StatusCode(200, cust);
        }
        
        [HttpDelete]
        [Route("Delete/{custId}")]
        public IActionResult Delete(int custId)
        {
            Customer deletingCustomer = dbContext.Customers.Where(e => e.CustomerId == custId).FirstOrDefault();
            if (deletingCustomer != null)
            {
                dbContext.Customers.Remove(deletingCustomer);
                // Cascade Delte
                dbContext.Rentals.RemoveRange(dbContext.Rentals.Where(e => e.Buyer.CustomerId == custId).ToArray());
                dbContext.SaveChanges();
                return StatusCode(200, "Delete Successful");
            }
            return StatusCode(400, "Delete Failed");
        }
    }
}
