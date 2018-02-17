using BikeRentalService.Controllers;
using BikeRentalService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class BikeRentalTests
    {
        private BikeRentalContext dbContext = new BikeRentalContext();
        private RentalsController rc = new RentalsController();

        [TestMethod]
        public void TestCalculation1()
        {
            var total=rc.CalculateTotalCost(new Rental()
            {
                RentedBike = new Bike
                {
                    RentalPriceFirstHour = 3,
                    RentalPriceExtraHour = 5
                },
                RentStartTime = new DateTime(2018, 2, 14, 8, 15, 0),
                RentEndTime=new DateTime(2018, 2, 14, 10, 30, 0)
            });

            Assert.AreEqual(13, total);
        }

        [TestMethod]
        public void TestCalculation2()
        {
            var total = rc.CalculateTotalCost(new Rental()
            {
                RentedBike = new Bike
                {
                    RentalPriceFirstHour = 3,
                    RentalPriceExtraHour = 100
                },
                RentStartTime = new DateTime(2018, 2, 14, 8, 15, 0),
                RentEndTime = new DateTime(2018, 2, 14, 8, 45, 0)
            });

            Assert.AreEqual(3, total);
        }

        [TestMethod]
        public void TestCalculation3()
        {
            var total = rc.CalculateTotalCost(new Rental()
            {
                RentedBike = new Bike
                {
                    RentalPriceFirstHour = 100,
                    RentalPriceExtraHour = 33
                },
                RentStartTime = new DateTime(2018, 2, 14, 8, 15, 0),
                RentEndTime = new DateTime(2018, 2, 14, 8, 25, 0)
            });

            Assert.AreEqual(0, total);
        }
    }
}
