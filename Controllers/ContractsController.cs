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
            var coliksContext = _context.Contracts.Include(c => c.Customer).Include(c => c.HelpStaff).Include(c => c.TuneStaff);
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

        // GET: Contracts/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Firstname");
            ViewData["HelpStaffId"] = new SelectList(_context.Staffs, "Id", "Id");
            ViewData["TuneStaffId"] = new SelectList(_context.Staffs, "Id", "Id");
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

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contracts = await _context.Contracts.FindAsync(id);
            _context.Contracts.Remove(contracts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractsExists(int id)
        {
            return _context.Contracts.Any(e => e.Id == id);
        }
    }
}
