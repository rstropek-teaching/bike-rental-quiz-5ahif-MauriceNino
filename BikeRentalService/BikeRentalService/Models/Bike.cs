using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalService.Models
{
    public class Bike
    {
        [Key]
        public int BikeId { get; set; }

        [Required]
        [StringLength(20)]
        public string Brand { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        public DateTime DateOfLastService { get; set; }

        [Required]
        [Range(0.00d, 9999d)]
        public double RentalPriceFirstHour { get; set; }

        [Required]
        [Range(1.00d, 9999d)]
        public double RentalPriceExtraHour { get; set; }

        public BikeCategory Category { get; set; }
    }
}
