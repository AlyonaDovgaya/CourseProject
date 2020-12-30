using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Infrastructure.EntityOperations;
using Warehouse.Infrastructure.ViewModels;
using Warehouse.Infrastructure.ViewModels.CustomerProducts;
using Warehouse.Models;

namespace Warehouse.Controllers
{
    public class CustomerProductsController : Controller
    {
        private readonly WarehouseContext _context;
        private readonly CustomerProductOperations _service;
        private readonly int _pageSize;

        public CustomerProductsController(WarehouseContext context)
        {
            _context = context;
            _service = new CustomerProductOperations();
            _pageSize = 8;
        }

        // GET: CustomerProducts
        public async Task<IActionResult> Index(string selectedCustomerName, int? page, SortViewModel.Sort? sort)
        {
            if (!User.IsInRole(Areas.Identity.Roles.User) && !User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return Redirect("~/Identity/Account/Login");
            }

            bool isFromFilter = HttpContext.Request.Query["isFromFilter"] == "true";

            _service.GetSortPagingCookiesForUserIfNull(Request.Cookies, User.Identity.Name, isFromFilter,
                ref page, ref sort);
            _service.GetFilterCookiesForUserIfNull(Request.Cookies, User.Identity.Name, isFromFilter,
                ref selectedCustomerName);
            _service.SetDefaultValuesIfNull(ref selectedCustomerName, ref page, ref sort);
            _service.SetCookies(Response.Cookies, User.Identity.Name, selectedCustomerName, page, sort);

            var customerProducts = _context.CustomerProducts.Include(c => c.Customer).Include(c => c.DeliveryMethod).Include(c => c.Employee).Include(c => c.Product).AsQueryable();

            customerProducts = _service.Filter(customerProducts, selectedCustomerName);

            var count = await customerProducts.CountAsync();

            customerProducts = _service.Sort(customerProducts, (SortViewModel.Sort)sort);
            customerProducts = _service.Paging(customerProducts, isFromFilter, (int)page, _pageSize);

            IndexViewModel model = new IndexViewModel
            {
                CustomerProducts = await customerProducts.ToListAsync(),
                PageViewModel = new PageViewModel(count, (int)page, _pageSize),
                FilterViewModel = new FilterViewModel(selectedCustomerName),
                SortViewModel = new SortViewModel((SortViewModel.Sort)sort),
            };

            return View(model);
        }

        // GET: CustomerProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.User) && !User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return Redirect("~/Identity/Account/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var customerProduct = await _context.CustomerProducts
                .Include(c => c.Customer)
                .Include(c => c.DeliveryMethod)
                .Include(c => c.Employee)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerProduct == null)
            {
                return NotFound();
            }

            return View(customerProduct);
        }

        // GET: CustomerProducts/Create
        public IActionResult Create()
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "CustomerProducts");
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "Name");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: CustomerProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,DepartureDate,Quantity,Price,CustomerId,ProductId,DeliveryMethodId,EmployeeId")] CustomerProduct customerProduct)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "CustomerProducts");
            }
            if (ModelState.IsValid)
            {
                _context.Add(customerProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", customerProduct.CustomerId);
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "Name", customerProduct.DeliveryMethodId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", customerProduct.EmployeeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", customerProduct.ProductId);
            return View(customerProduct);
        }

        // GET: CustomerProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "CustomerProducts");
            }
            if (id == null)
            {
                return NotFound();
            }

            var customerProduct = await _context.CustomerProducts.FindAsync(id);
            if (customerProduct == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", customerProduct.CustomerId);
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "Name", customerProduct.DeliveryMethodId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", customerProduct.EmployeeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", customerProduct.ProductId);
            return View(customerProduct);
        }

        // POST: CustomerProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,DepartureDate,Quantity,Price,CustomerId,ProductId,DeliveryMethodId,EmployeeId")] CustomerProduct customerProduct)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "CustomerProducts");
            }
            if (id != customerProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerProductExists(customerProduct.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", customerProduct.CustomerId);
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "Name", customerProduct.DeliveryMethodId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", customerProduct.EmployeeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", customerProduct.ProductId);
            return View(customerProduct);
        }

        // GET: CustomerProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "CustomerProducts");
            }
            if (id == null)
            {
                return NotFound();
            }

            var customerProduct = await _context.CustomerProducts
                .Include(c => c.Customer)
                .Include(c => c.DeliveryMethod)
                .Include(c => c.Employee)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerProduct == null)
            {
                return NotFound();
            }

            return View(customerProduct);
        }

        // POST: CustomerProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "CustomerProducts");
            }
            var customerProduct = await _context.CustomerProducts.FindAsync(id);
            _context.CustomerProducts.Remove(customerProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerProductExists(int id)
        {
            return _context.CustomerProducts.Any(e => e.Id == id);
        }
    }
}
