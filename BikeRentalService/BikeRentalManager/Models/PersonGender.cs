using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Models
{
    public class PersonGender
    {
        [Key]
        public int GenderId { get; set; }

        [Required]
        public string GenderName { get; set; }
    }
}