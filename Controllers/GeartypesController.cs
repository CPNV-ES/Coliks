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
    public class GeartypesController : Controller
    {
        private readonly ColiksContext _context;

        public GeartypesController(ColiksContext context)
        {
            _context = context;
        }

        // GET: Geartypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Geartypes.ToListAsync());
        }

        // GET: Geartypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var geartypes = await _context.Geartypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (geartypes == null)
            {
                return NotFound();
            }

            return View(geartypes);
        }

        // GET: Geartypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Geartypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Geartypes geartypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(geartypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(geartypes);
        }

        // GET: Geartypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var geartypes = await _context.Geartypes.FindAsync(id);
            if (geartypes == null)
            {
                return NotFound();
            }
            return View(geartypes);
        }

        // POST: Geartypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Geartypes geartypes)
        {
            if (id != geartypes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(geartypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeartypesExists(geartypes.Id))
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
            return View(geartypes);
        }

        // GET: Geartypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var geartypes = await _context.Geartypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (geartypes == null)
            {
                return NotFound();
            }

            return View(geartypes);
        }

        // POST: Geartypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var geartypes = await _context.Geartypes.FindAsync(id);
            _context.Geartypes.Remove(geartypes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GeartypesExists(int id)
        {
            return _context.Geartypes.Any(e => e.Id == id);
        }
    }
}
