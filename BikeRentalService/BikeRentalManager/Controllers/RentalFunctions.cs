using BikeRentalService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeRentalManager.Controllers
{
    public class RentalFunctions
    {
        // Function to calculate the cost for a rental
        public static double CalculateTotalCost(Rental rent)
        {
            TimeSpan timeDiff = ((DateTime)rent.RentEndTime - (DateTime)rent.RentStartTime);
            var totalMinutes = timeDiff.TotalMinutes;
            var startedHours = (int)Math.Ceiling(((double)totalMinutes) / 60);

            return totalMinutes <= 15 ?
                0 :
                rent.RentedBike.RentalPriceFirstHour + (rent.RentedBike.RentalPriceExtraHour * (startedHours - 1));
        }
    }
}
