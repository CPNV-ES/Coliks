using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using coliks.Models;

namespace coliks.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ColiksContext _context;

        public ContractsController(ColiksContext context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index()
        {
            var coliksContext = _context.Contracts.Where(c => c.Effectivereturn == null).Include(c => c.Customer).Include(c => c.HelpStaff).Include(c => c.TuneStaff);
            return View(await coliksContext.ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contracts = await _context.Contracts
                .Include(c => c.Customer)
                .Include(c => c.HelpStaff)
                .Include(c => c.TuneStaff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contracts == null)
            {
                return NotFound();
            }

            return View(contracts);
        }

        // View to display all contracts that were not paid and that are not returned after the planned date
        public IActionResult NotPaidOrOpen()
        {
            // Get all the contracts where there's no Paidon date (means it's not paid)
            ViewData["NotPaid"] = _context.Contracts.Where(c => c.Paidon == null).Include(c => c.Customer).Take(50).ToList();
            // Get all the contracts that have no effective return date and where the planned date is in the past
            ViewData["Open"] = _context.Contracts.Where(c => c.Effectivereturn == null && c.Plannedreturn < DateTime.Now).Include(c => c.Customer).Take(50).ToList();
            return View();
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {

            ViewData["CustomersLastName"] = _context.Customers.OrderBy(c => c.Lastname).Select(c => c.Lastname).Distinct().ToList();
            ViewData["HelpStaffId"] = new SelectList(_context.Staffs, "Id", "Nom");
            ViewData["TuneStaffId"] = new SelectList(_context.Staffs, "Id", "Nom");
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Creationdate,Effectivereturn,Plannedreturn,CustomerId,Notes,Total,Takenon,Paidon,Insurance,Goget,HelpStaffId,TuneStaffId")] Contracts contracts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contracts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Firstname", contracts.CustomerId);
            ViewData["HelpStaffId"] = new SelectList(_context.Staffs, "Id", "Id", contracts.HelpStaffId);
            ViewData["TuneStaffId"] = new SelectList(_context.Staffs, "Id", "Id", contracts.TuneStaffId);
            return View(contracts);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contracts = await _context.Contracts.FindAsync(id);
            if (contracts == null)
            {
                return NotFound();
            }
            if (contracts.Effectivereturn != null)
            {
                return RedirectToAction(nameof(Details), new { id = contracts.Id });
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Firstname", contracts.CustomerId);
            ViewData["HelpStaffId"] = new SelectList(_context.Staffs, "Id", "Id", contracts.HelpStaffId);
            ViewData["TuneStaffId"] = new SelectList(_context.Staffs, "Id", "Id", contracts.TuneStaffId);
            return View(contracts);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Creationdate,Effectivereturn,Plannedreturn,CustomerId,Notes,Total,Takenon,Paidon,Insurance,Goget,HelpStaffId,TuneStaffId")] Contracts contracts)
        {
            if (id != contracts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contracts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractsExists(contracts.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Firstname", contracts.CustomerId);
            ViewData["HelpStaffId"] = new SelectList(_context.Staffs, "Id", "Id", contracts.HelpStaffId);
            ViewData["TuneStaffId"] = new SelectList(_context.Staffs, "Id", "Id", contracts.TuneStaffId);
            return View(contracts);
        }

        [HttpPost]
        public async Task<IActionResult> SoftDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts.FindAsync(id);
            contract.Effectivereturn = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = contract.Id });
        }

        private bool ContractsExists(int id)
        {
            return _context.Contracts.Any(e => e.Id == id);
        }
    }
}
