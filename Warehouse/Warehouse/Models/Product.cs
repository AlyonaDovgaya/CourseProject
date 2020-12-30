using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Product")]
        [MinLength(5, ErrorMessage = "Short name. Min length must be 5")]
        [MaxLength(50, ErrorMessage = "Long name. Max length must be 50")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Storage conditions")]
        [MinLength(5, ErrorMessage = "Short name of storage condition. Min length must be 5")]
        [MaxLength(150, ErrorMessage = "Long name of storage condition. Max length must be 150")]
        public string StorageConditions { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Short name of packaging. Min length must be 5")]
        [MaxLength(100, ErrorMessage = "Long name of packaging. Max length must be 100")]
        public string Packaging { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DisplayName("Expiry date")]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 10001, ErrorMessage = "Product price must be in range from 1 to 10000")]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Product type")]
        [Range(1, int.MaxValue, ErrorMessage = "Select product type")]
        public int? ProductTypeId { get; set; }

        [Required]
        [DisplayName("Manufacturer")]
        [Range(1, int.MaxValue, ErrorMessage = "Select manufacturer")]
        public int? ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        public virtual ProductType ProductType { get; set; }

    }
}
