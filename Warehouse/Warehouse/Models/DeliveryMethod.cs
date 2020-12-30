using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Warehouse.Models
{
    public class DeliveryMethod
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Delivery method")]
        [MinLength(5, ErrorMessage = "Short name of delivery method. Min length must be 5")]
        [MaxLength(100, ErrorMessage = "Long name of delivery method. Max length must be 100")]
        public string Name { get; set; }
    }
}
