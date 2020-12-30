using System;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Data
{
    public class DbInitializer
    {
        readonly int _referenceTableSize;
        readonly int _operationalTableSize;
        public DbInitializer(int referenceTableSize = 100, int operationalTableSize = 10000)
        {
            _referenceTableSize = referenceTableSize;
            _operationalTableSize = operationalTableSize;
        }
        public async Task Initialize(WarehouseContext dbContext)
        {
            Random rand = new Random();
            if (!dbContext.DeliveryMethods.Any())
            {
                for (int i = 0; i < _referenceTableSize; i++)
                {
                    await dbContext.DeliveryMethods.AddAsync(new Models.DeliveryMethod
                    {
                        Name = GetRandomString(100)
                    });
                }
            }
            await dbContext.SaveChangesAsync();

            if (!dbContext.Manufacturers.Any())
            {
                for (int i = 0; i < _referenceTableSize; i++)
                {
                    await dbContext.Manufacturers.AddAsync(new Models.Manufacturer
                    {
                        Name = GetRandomString(50)
                    });
                }
            }
            await dbContext.SaveChangesAsync();

            if (!dbContext.ProductTypes.Any())
            {
                for (int i = 0; i < _referenceTableSize; i++)
                {
                    await dbContext.ProductTypes.AddAsync(new Models.ProductType
                    {
                        Name = GetRandomString(50),
                        Description = GetRandomString(150),
                        Features = GetRandomString(150)
                    });
                }
            }
            await dbContext.SaveChangesAsync();

            if (!dbContext.Products.Any())
            {
                var productTypes = dbContext.ProductTypes.ToList();
                var manufacturers = dbContext.Manufacturers.ToList();
                for (int i = 0; i < _operationalTableSize; i++)
                {
                    var productType = productTypes.ElementAt(rand.Next(dbContext.ProductTypes.Count() - 1));
                    var manufacturer = manufacturers.ElementAt(rand.Next(dbContext.Manufacturers.Count() - 1));
                    await dbContext.Products.AddAsync(new Models.Product
                    {
                        Name = GetRandomString(50),
                        StorageConditions = GetRandomString(150),
                        Packaging = GetRandomString(100),
                        ExpiryDate = GetRandomDate(new DateTime(2000, 1, 1), DateTime.Now),
                        Price = rand.Next(200, 3000),

                        ProductType = productType,
                        ProductTypeId = productType.Id,
                        Manufacturer = manufacturer,
                        ManufacturerId = manufacturer.Id
                    });
                }
            }
            await dbContext.SaveChangesAsync();

            if (!dbContext.Customers.Any())
            {
                for (int i = 0; i < _referenceTableSize; i++)
                {
                    await dbContext.Customers.AddAsync(new Models.Customer
                    {
                        Name = GetRandomString(50),
                        Address = GetRandomString(100),
                        Phone = rand.Next(1000000, 9999999)
                    });
                }
            }
            await dbContext.SaveChangesAsync();

            if (!dbContext.Employees.Any())
            {
                for (int i = 0; i < _referenceTableSize; i++)
                {
                    await dbContext.Employees.AddAsync(new Models.Employee
                    {
                        Name = GetRandomString(50),
                    });
                }
            }
            await dbContext.SaveChangesAsync();

            if (!dbContext.Suppliers.Any())
            {
                for (int i = 0; i < _referenceTableSize; i++)
                {
                    await dbContext.Suppliers.AddAsync(new Models.Supplier
                    {
                        Name = GetRandomString(50),
                        Address = GetRandomString(100),
                        Phone = rand.Next(1000000, 9999999)
                    });
                }
            }
            await dbContext.SaveChangesAsync();

            if (!dbContext.SupplierProducts.Any())
            {
                var deliveryMethods = dbContext.DeliveryMethods.ToList();
                var employees = dbContext.Employees.ToList();
                var products = dbContext.Products.ToList();
                var suppliers = dbContext.Suppliers.ToList();
                for (int i = 0; i < _operationalTableSize; i++)
                {
                    await dbContext.SupplierProducts.AddAsync(new Models.SupplierProduct
                    {
                        ReceiptDate = GetRandomDate(new DateTime(2000, 1, 1), DateTime.Now),
                        Quantity = rand.Next(1, 500),
                        Price = rand.Next(1, 1000),

                        ProductId = products.ElementAt(rand.Next(dbContext.Products.Count() - 1)).Id,
                        SupplierId = suppliers.ElementAt(rand.Next(dbContext.Suppliers.Count() - 1)).Id,
                        EmployeeId = employees.ElementAt(rand.Next(dbContext.Employees.Count() - 1)).Id,
                        DeliveryMethodId = deliveryMethods.ElementAt(rand.Next(dbContext.DeliveryMethods.Count() - 1)).Id
                    });
                }
            }
            await dbContext.SaveChangesAsync();
            if (!dbContext.CustomerProducts.Any())
            {
                var deliveryMethods = dbContext.DeliveryMethods.ToList();
                var employees = dbContext.Employees.ToList();
                var products = dbContext.Products.ToList();
                var customers = dbContext.Customers.ToList();
                for (int i = 0; i < _operationalTableSize; i++)
                {
                    var orderDate = GetRandomDate(new DateTime(2000, 1, 1), new DateTime(2020, 1, 1));
                    await dbContext.CustomerProducts.AddAsync(new Models.CustomerProduct
                    {
                        OrderDate = orderDate,
                        DepartureDate = orderDate.AddMonths(rand.Next(0, 9)).AddDays(rand.Next(0, 28)),
                        Quantity = rand.Next(1, 500),
                        Price = rand.Next(1, 1000),

                        ProductId = products.ElementAt(rand.Next(dbContext.Products.Count() - 1)).Id,
                        CustomerId = customers.ElementAt(rand.Next(dbContext.Customers.Count() - 1)).Id,
                        EmployeeId = employees.ElementAt(rand.Next(dbContext.Employees.Count() - 1)).Id,
                        DeliveryMethodId = deliveryMethods.ElementAt(rand.Next(dbContext.DeliveryMethods.Count() - 1)).Id

                    });
                }
            }
            await dbContext.SaveChangesAsync();
        }
        public string GetRandomString(int maxLength)
        {
            Random rand = new Random();
            int length = rand.Next(maxLength / 3, maxLength);
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var str = new char[length];
            int spaceRange = rand.Next(4, 7);
            for (int i = 0; i < length; i++)
            {
                if ((i + 1) % spaceRange == 0)
                {
                    str[i] = ' ';
                    spaceRange = rand.Next(4, 7);
                    continue;
                }
                str[i] = chars[rand.Next(chars.Length)];
            }
            return new string(str);
        }
        public DateTime GetRandomDate(DateTime minDate, DateTime maxDate)
        {
            Random rand = new Random();
            int range = (maxDate - minDate).Days;
            return minDate.AddDays(rand.Next(range));
        }
    }
}