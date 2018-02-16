using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalService.Models
{
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }

        [Required]
        public Customer Buyer { get; set; }

        [Required]
        public Bike RentedBike { get; set; }

        [Required]
        public DateTime RentStartTime { get; set; }

        public DateTime RentEndTime { get; set; }

        [Range(1d, 99999999d)]
        public double TotalCost { get; set; }

        public bool PaidFlag { get; set; }
    }
}
