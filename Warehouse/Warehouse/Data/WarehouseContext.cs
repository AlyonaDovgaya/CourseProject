using Microsoft.EntityFrameworkCore;

namespace Warehouse.Data
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext(DbContextOptions<WarehouseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Models.Customer> Customers { get; set; }
        public DbSet<Models.CustomerProduct> CustomerProducts { get; set; }
        public DbSet<Models.DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<Models.Employee> Employees { get; set; }
        public DbSet<Models.Manufacturer> Manufacturers { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.ProductType> ProductTypes { get; set; }
        public DbSet<Models.Supplier> Suppliers { get; set; }
        public DbSet<Models.SupplierProduct> SupplierProducts { get; set; }
    }
}
