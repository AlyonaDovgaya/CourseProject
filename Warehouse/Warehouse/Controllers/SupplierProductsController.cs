using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Infrastructure.EntityOperations;
using Warehouse.Infrastructure.ViewModels;
using Warehouse.Infrastructure.ViewModels.SupplierProducts;
using Warehouse.Models;

namespace Warehouse.Controllers
{
    public class SupplierProductsController : Controller
    {
        private readonly WarehouseContext _context;
        private readonly SupplierProductOperations _service;
        private readonly int _pageSize;

        public SupplierProductsController(WarehouseContext context)
        {
            _context = context;
            _service = new SupplierProductOperations();
            _pageSize = 8;
        }

        // GET: SupplierProducts
        public async Task<IActionResult> Index(string selectedSupplierName, int? page, SortViewModel.Sort? sort)
        {
            if (!User.IsInRole(Areas.Identity.Roles.User) && !User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return Redirect("~/Identity/Account/Login");
            }

            bool isFromFilter = HttpContext.Request.Query["isFromFilter"] == "true";

            _service.GetSortPagingCookiesForUserIfNull(Request.Cookies, User.Identity.Name, isFromFilter,
                ref page, ref sort);
            _service.GetFilterCookiesForUserIfNull(Request.Cookies, User.Identity.Name, isFromFilter,
                ref selectedSupplierName);
            _service.SetDefaultValuesIfNull(ref selectedSupplierName, ref page, ref sort);
            _service.SetCookies(Response.Cookies, User.Identity.Name, selectedSupplierName, page, sort);

            var supplierProducts = _context.SupplierProducts.Include(s => s.DeliveryMethod).Include(s => s.Employee).Include(s => s.Product).Include(s => s.Supplier).AsQueryable();

            supplierProducts = _service.Filter(supplierProducts, selectedSupplierName);

            var count = await supplierProducts.CountAsync();

            supplierProducts = _service.Sort(supplierProducts, (SortViewModel.Sort)sort);
            supplierProducts = _service.Paging(supplierProducts, isFromFilter, (int)page, _pageSize);

            IndexViewModel model = new IndexViewModel
            {
                SupplierProducts = await supplierProducts.ToListAsync(),
                PageViewModel = new PageViewModel(count, (int)page, _pageSize),
                FilterViewModel = new FilterViewModel(selectedSupplierName),
                SortViewModel = new SortViewModel((SortViewModel.Sort)sort),
            };

            return View(model);
        }

        // GET: SupplierProducts/Details/5
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

            var supplierProduct = await _context.SupplierProducts
                .Include(s => s.DeliveryMethod)
                .Include(s => s.Employee)
                .Include(s => s.Product)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplierProduct == null)
            {
                return NotFound();
            }

            return View(supplierProduct);
        }

        // GET: SupplierProducts/Create
        public IActionResult Create()
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "SupplierProducts");
            }
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "Name");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name");
            return View();
        }

        // POST: SupplierProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReceiptDate,Quantity,Price,SupplierId,ProductId,DeliveryMethodId,EmployeeId")] SupplierProduct supplierProduct)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "SupplierProducts");
            }
            if (ModelState.IsValid)
            {
                _context.Add(supplierProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "Name", supplierProduct.DeliveryMethodId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", supplierProduct.EmployeeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", supplierProduct.ProductId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", supplierProduct.SupplierId);
            return View(supplierProduct);
        }

        // GET: SupplierProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "SupplierProducts");
            }
            if (id == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SupplierProducts.FindAsync(id);
            if (supplierProduct == null)
            {
                return NotFound();
            }
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "Name", supplierProduct.DeliveryMethodId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", supplierProduct.EmployeeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", supplierProduct.ProductId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", supplierProduct.SupplierId);
            return View(supplierProduct);
        }

        // POST: SupplierProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReceiptDate,Quantity,Price,SupplierId,ProductId,DeliveryMethodId,EmployeeId")] SupplierProduct supplierProduct)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "SupplierProducts");
            }
            if (id != supplierProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplierProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierProductExists(supplierProduct.Id))
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
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "Name", supplierProduct.DeliveryMethodId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", supplierProduct.EmployeeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", supplierProduct.ProductId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", supplierProduct.SupplierId);
            return View(supplierProduct);
        }

        // GET: SupplierProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "SupplierProducts");
            }
            if (id == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SupplierProducts
                .Include(s => s.DeliveryMethod)
                .Include(s => s.Employee)
                .Include(s => s.Product)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplierProduct == null)
            {
                return NotFound();
            }

            return View(supplierProduct);
        }

        // POST: SupplierProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "SupplierProducts");
            }
            var supplierProduct = await _context.SupplierProducts.FindAsync(id);
            _context.SupplierProducts.Remove(supplierProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierProductExists(int id)
        {
            return _context.SupplierProducts.Any(e => e.Id == id);
        }
    }
}
