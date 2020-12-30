using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Employee")]
        [MinLength(5, ErrorMessage = "Short name. Min length must be 5")]
        [MaxLength(50, ErrorMessage = "Long name. Max length must be 50")]
        public string Name { get; set; }
    }
}
