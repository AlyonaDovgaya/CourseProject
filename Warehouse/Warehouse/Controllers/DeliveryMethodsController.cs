using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Infrastructure.EntityOperations;
using Warehouse.Infrastructure.ViewModels;
using Warehouse.Infrastructure.ViewModels.DeliveryMethods;
using Warehouse.Models;

namespace Warehouse.Controllers
{
    public class DeliveryMethodsController : Controller
    {
        private readonly WarehouseContext _context;
        private readonly DeliveryMethodOperations _service;
        private readonly int _pageSize;

        public DeliveryMethodsController(WarehouseContext context)
        {
            _context = context;
            _service = new DeliveryMethodOperations();
            _pageSize = 8;
        }

        // GET: DeliveryMethods
        public async Task<IActionResult> Index(string selectedName, int? page, SortViewModel.Sort? sort)
        {
            if (!User.IsInRole(Areas.Identity.Roles.User) && !User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return Redirect("~/Identity/Account/Login");
            }

            bool isFromFilter = HttpContext.Request.Query["isFromFilter"] == "true";

            _service.GetSortPagingCookiesForUserIfNull(Request.Cookies, User.Identity.Name, isFromFilter,
                ref page, ref sort);
            _service.GetFilterCookiesForUserIfNull(Request.Cookies, User.Identity.Name, isFromFilter,
                ref selectedName);
            _service.SetDefaultValuesIfNull(ref selectedName, ref page, ref sort);
            _service.SetCookies(Response.Cookies, User.Identity.Name, selectedName, page, sort);

            var deliveryMethods = _context.DeliveryMethods.AsQueryable();

            deliveryMethods = _service.Filter(deliveryMethods, selectedName);

            var count = await deliveryMethods.CountAsync();

            deliveryMethods = _service.Sort(deliveryMethods, (SortViewModel.Sort)sort);
            deliveryMethods = _service.Paging(deliveryMethods, isFromFilter, (int)page, _pageSize);

            IndexViewModel model = new IndexViewModel
            {
                DeliveryMethods = await deliveryMethods.ToListAsync(),
                PageViewModel = new PageViewModel(count, (int)page, _pageSize),
                FilterViewModel = new FilterViewModel(selectedName),
                SortViewModel = new SortViewModel((SortViewModel.Sort)sort),
            };

            return View(model);
        }


        // GET: DeliveryMethods/Details/5
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

            var deliveryMethod = await _context.DeliveryMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryMethod == null)
            {
                return NotFound();
            }

            return View(deliveryMethod);
        }

        // GET: DeliveryMethods/Create
        public IActionResult Create()
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "DeliveryMethods");
            }
            return View();
        }

        // POST: DeliveryMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] DeliveryMethod deliveryMethod)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "DeliveryMethods");
            }
            if (ModelState.IsValid)
            {
                _context.Add(deliveryMethod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryMethod);
        }

        // GET: DeliveryMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "DeliveryMethods");
            }
            if (id == null)
            {
                return NotFound();
            }

            var deliveryMethod = await _context.DeliveryMethods.FindAsync(id);
            if (deliveryMethod == null)
            {
                return NotFound();
            }
            return View(deliveryMethod);
        }

        // POST: DeliveryMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] DeliveryMethod deliveryMethod)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "DeliveryMethods");
            }
            if (id != deliveryMethod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryMethodExists(deliveryMethod.Id))
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
            return View(deliveryMethod);
        }

        // GET: DeliveryMethods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "DeliveryMethods");
            }
            if (id == null)
            {
                return NotFound();
            }

            var deliveryMethod = await _context.DeliveryMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryMethod == null)
            {
                return NotFound();
            }

            return View(deliveryMethod);
        }

        // POST: DeliveryMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "DeliveryMethods");
            }
            var deliveryMethod = await _context.DeliveryMethods.FindAsync(id);
            _context.DeliveryMethods.Remove(deliveryMethod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryMethodExists(int id)
        {
            return _context.DeliveryMethods.Any(e => e.Id == id);
        }
    }
}
