using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalService.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public PersonGender Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(75)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        [StringLength(75)]
        public string Street { get; set; }
        
        [Range(0, 10 * 10 - 1)]
        public int HouseNumber { get; set; }

        [Required]
        [Range(0, 10 * 10 - 1)]
        public int ZipCode { get; set; }

        [Required]
        [StringLength(75)]
        public string Town { get; set; }
    }
}
