using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models
{
    public class ProductType
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Product Type")]
        [MinLength(5, ErrorMessage = "Short name. Min length must be 5")]
        [MaxLength(50, ErrorMessage = "Long name. Max length must be 50")]
        public string Name { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Short description. Min length must be 5")]
        [MaxLength(150, ErrorMessage = "Long description. Max length must be 150")]
        public string Description { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Short features")]
        [MaxLength(150, ErrorMessage = "Long features")]
        public string Features { get; set; }
    }
}
