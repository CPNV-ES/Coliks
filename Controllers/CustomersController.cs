using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using coliks.Models;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Routing;

namespace coliks.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ColiksContext _context;

        public CustomersController(ColiksContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get: Customers list using pagination and sorting method
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sortExpression"></param>
        /// <param name="filter"</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(string filter, int page = 1, string sortExpression = "Lastname")
        {
            // lines par page
            var lines = 10;    
            var coliksContext = _context.Customers.Include(c => c.City).AsNoTracking().OrderBy(p => p.Lastname).AsQueryable(); ;

            // criteria to use in the seach filter (search value in firstname, lastname, email, address, etc.)
            if (!string.IsNullOrWhiteSpace(filter))
                coliksContext = coliksContext.Where(p => 
                    p.Lastname.Contains(filter) || 
                    p.Firstname.Contains(filter) ||
                    p.Address.Contains(filter) ||
                    p.Email.Contains(filter) ||
                    p.Phone.Contains(filter) ||
                    p.Mobile.Contains(filter) ||
                    p.City.Name.Contains(filter) ||
                   ( (p.Firstname + " " + p.Lastname).Contains(filter)) ||
                   ( (p.Lastname + " " + p.Firstname).Contains(filter)));

            // prepare model with pagination
            var model = await PagingList.CreateAsync(coliksContext, lines, page, sortExpression, "Lastname");
            //apply filter
            model.RouteValue = new RouteValueDictionary {{ "filter", filter}};
            return View(model);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult VerifyName(string firstName, string lastName)
        {
            var coliksContext = _context.Customers.AsQueryable();
            coliksContext = coliksContext.Where(p => p.Lastname.Equals(lastName) && p.Firstname.Equals(firstName));

            if (coliksContext.Count() != 0)
            {
                return Json(data: false);
            }

            return Json(data: true);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .Include(c => c.City)
                .Include( c=> c.Purchases)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customers == null)
            {
                return NotFound();
            }

            List<Purchases> elementsAfterLastDiscount = new List<Purchases>();

            // Calculate total client's purchases
            // There ara two way to calculate:
            //  - If exists a vaucher -> then sum of all client's purchases from the last vaucher
            //  - If a vaucher don't exists -> then sum of all client's purchases
            // check if exsist a discoun and get the index value

            if (customers.Purchases != null)
            {
                // get all customers phurchases
                List<Purchases> purchases = customers.Purchases.OrderBy(c => c.Date).ToList();

                //get the index of the last vaucher
                int index = purchases.FindIndex(a => a.Description == "*** Bon d-achat de 50.- ***");

                if (index != -1) //calculate client's purchases total from the last vaucher
                {
                    // get the last vaucher
                    int lastIndexDiscount = purchases.IndexOf(purchases.Where(c => c.Description == "*** Bon d-achat de 50.- ***").Last());

                    // get last purchases from the last vaucher
                    for (int i = lastIndexDiscount+1; i < purchases.Count(); i++)
                    {
                        elementsAfterLastDiscount.Add(purchases[i]);
                    }
                }
                else // calculate the client's total purchases if any vaucher exists
                {
                    elementsAfterLastDiscount = purchases;
                }
            }

            ViewBag.totalPurchase = elementsAfterLastDiscount.Sum(item => item.Amount);
            ViewBag.isReduction = ViewBag.totalPurchase >= 500 ? true : false;

            var tuple = new Tuple<Customers, Purchases>(customers, null);

            return View(tuple);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Lastname,Firstname,Address,CityId,Phone,Email,Mobile")] Customers customers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", customers.CityId);
            return View(customers);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", customers.CityId);
            return View(customers);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Lastname,Firstname,Address,CityId,Phone,Email,Mobile")] Customers customers)
        {
            if (id != customers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomersExists(customers.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", customers.CityId);
            return View(customers);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customers = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
