using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Models
{
    public class BikeCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}