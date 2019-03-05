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
    public class RentpricesController : Controller
    {
        private readonly ColiksContext _context;

        public RentpricesController(ColiksContext context)
        {
            _context = context;
        }

        // GET: Rentprices
        public async Task<IActionResult> Index()
        {
            var coliksContext = _context.Rentprices.Include(r => r.Category).Include(r => r.Duration).Include(r => r.Geartype);
            return View(await coliksContext.ToListAsync());
        }

        // GET: Rentprices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentprices = await _context.Rentprices
                .Include(r => r.Category)
                .Include(r => r.Duration)
                .Include(r => r.Geartype)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentprices == null)
            {
                return NotFound();
            }

            return View(rentprices);
        }

        // GET: Rentprices/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code");
            ViewData["DurationId"] = new SelectList(_context.Durations, "Id", "Code");
            ViewData["GeartypeId"] = new SelectList(_context.Geartypes, "Id", "Name");
            return View();
        }

        // POST: Rentprices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,DurationId,GeartypeId,Price")] Rentprices rentprices)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentprices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code", rentprices.CategoryId);
            ViewData["DurationId"] = new SelectList(_context.Durations, "Id", "Code", rentprices.DurationId);
            ViewData["GeartypeId"] = new SelectList(_context.Geartypes, "Id", "Name", rentprices.GeartypeId);
            return View(rentprices);
        }

        // GET: Rentprices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentprices = await _context.Rentprices.FindAsync(id);
            if (rentprices == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code", rentprices.CategoryId);
            ViewData["DurationId"] = new SelectList(_context.Durations, "Id", "Code", rentprices.DurationId);
            ViewData["GeartypeId"] = new SelectList(_context.Geartypes, "Id", "Name", rentprices.GeartypeId);
            return View(rentprices);
        }

        // POST: Rentprices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,DurationId,GeartypeId,Price")] Rentprices rentprices)
        {
            if (id != rentprices.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentprices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentpricesExists(rentprices.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code", rentprices.CategoryId);
            ViewData["DurationId"] = new SelectList(_context.Durations, "Id", "Code", rentprices.DurationId);
            ViewData["GeartypeId"] = new SelectList(_context.Geartypes, "Id", "Name", rentprices.GeartypeId);
            return View(rentprices);
        }

        // GET: Rentprices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentprices = await _context.Rentprices
                .Include(r => r.Category)
                .Include(r => r.Duration)
                .Include(r => r.Geartype)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentprices == null)
            {
                return NotFound();
            }

            return View(rentprices);
        }

        // POST: Rentprices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentprices = await _context.Rentprices.FindAsync(id);
            _context.Rentprices.Remove(rentprices);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentpricesExists(int id)
        {
            return _context.Rentprices.Any(e => e.Id == id);
        }
    }
}
