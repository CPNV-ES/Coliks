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
    public class RenteditemsController : Controller
    {
        private readonly ColiksContext _context;

        public RenteditemsController(ColiksContext context)
        {
            _context = context;
        }

        // GET: Renteditems
        public async Task<IActionResult> Index()
        {
            var coliksContext = _context.Renteditems.Include(r => r.Category).Include(r => r.Contract).Include(r => r.Duration).Include(r => r.Item);
            return View(await coliksContext.ToListAsync());
        }

        // GET: Renteditems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renteditems = await _context.Renteditems
                .Include(r => r.Category)
                .Include(r => r.Contract)
                .Include(r => r.Duration)
                .Include(r => r.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (renteditems == null)
            {
                return NotFound();
            }

            return View(renteditems);
        }

        // GET: Renteditems/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code");
            ViewData["ContractId"] = new SelectList(_context.Contracts, "Id", "Id");
            ViewData["DurationId"] = new SelectList(_context.Durations, "Id", "Code");
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Itemnb");
            return View();
        }

        // POST: Renteditems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,ContractId,DurationId,CategoryId,Price,Description,Linenb,Partialreturn")] Renteditems renteditems)
        {
            if (ModelState.IsValid)
            {
                _context.Add(renteditems);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code", renteditems.CategoryId);
            ViewData["ContractId"] = new SelectList(_context.Contracts, "Id", "Id", renteditems.ContractId);
            ViewData["DurationId"] = new SelectList(_context.Durations, "Id", "Code", renteditems.DurationId);
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Itemnb", renteditems.ItemId);
            return View(renteditems);
        }

        // GET: Renteditems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renteditems = await _context.Renteditems.FindAsync(id);
            if (renteditems == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code", renteditems.CategoryId);
            ViewData["ContractId"] = new SelectList(_context.Contracts, "Id", "Id", renteditems.ContractId);
            ViewData["DurationId"] = new SelectList(_context.Durations, "Id", "Code", renteditems.DurationId);
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Itemnb", renteditems.ItemId);
            return View(renteditems);
        }

        // POST: Renteditems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemId,ContractId,DurationId,CategoryId,Price,Description,Linenb,Partialreturn")] Renteditems renteditems)
        {
            if (id != renteditems.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(renteditems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RenteditemsExists(renteditems.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code", renteditems.CategoryId);
            ViewData["ContractId"] = new SelectList(_context.Contracts, "Id", "Id", renteditems.ContractId);
            ViewData["DurationId"] = new SelectList(_context.Durations, "Id", "Code", renteditems.DurationId);
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Itemnb", renteditems.ItemId);
            return View(renteditems);
        }

        // GET: Renteditems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renteditems = await _context.Renteditems
                .Include(r => r.Category)
                .Include(r => r.Contract)
                .Include(r => r.Duration)
                .Include(r => r.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (renteditems == null)
            {
                return NotFound();
            }

            return View(renteditems);
        }

        // POST: Renteditems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var renteditems = await _context.Renteditems.FindAsync(id);
            _context.Renteditems.Remove(renteditems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RenteditemsExists(int id)
        {
            return _context.Renteditems.Any(e => e.Id == id);
        }
    }
}
