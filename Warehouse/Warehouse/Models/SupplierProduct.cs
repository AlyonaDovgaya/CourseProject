using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models
{
    public class SupplierProduct
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Receipt date")]
        public DateTime ReceiptDate { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Minimum value of quantity is 1")]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 10001, ErrorMessage = "Product price must be in range from 1 to 10000")]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Supplier")]
        [Range(1, int.MaxValue, ErrorMessage = "Select supplier")]
        public int? SupplierId { get; set; }

        [Required]
        [DisplayName("Product")]
        [Range(1, int.MaxValue, ErrorMessage = "Select product")]
        public int? ProductId { get; set; }

        [Required]
        [DisplayName("Delivery method")]
        [Range(1, int.MaxValue, ErrorMessage = "Select delivery method")]
        public int? DeliveryMethodId { get; set; }
        [Required]
        [DisplayName("Employee")]
        [Range(1, int.MaxValue, ErrorMessage = "Select employee")]
        public int? EmployeeId { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual Product Product { get; set; }
        public virtual DeliveryMethod DeliveryMethod { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
