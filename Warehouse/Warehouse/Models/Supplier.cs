using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Supplier")]
        [MinLength(5, ErrorMessage = "Short name. Min length must be 5")]
        [MaxLength(50, ErrorMessage = "Long name. Max length must be 50")]
        public string Name { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Short address. Min length must be 5")]
        [MaxLength(100, ErrorMessage = "Long address. Max length must be 100")]
        public string Address { get; set; }

        [Required]
        [Range(1000000, 9999999, ErrorMessage = "Phone number must be in range from 1000000 to 9999999")]
        public int Phone { get; set; }
    }
}
