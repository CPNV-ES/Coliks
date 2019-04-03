using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using coliks.Models;

namespace coliks
{
    public class CustomerCategoriesController : Controller
    {
        private readonly ColiksContext _context;

        public CustomerCategoriesController(ColiksContext context)
        {
            _context = context;
        }

        // GET: CustomerCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomerCategories.ToListAsync());
        }

        // GET: CustomerCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerCategories = await _context.CustomerCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerCategories == null)
            {
                return NotFound();
            }

            return View(customerCategories);
        }

        // GET: CustomerCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Totalamount,Categoryname")] CustomerCategories customerCategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerCategories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerCategories);
        }

        // GET: CustomerCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerCategories = await _context.CustomerCategories.FindAsync(id);
            if (customerCategories == null)
            {
                return NotFound();
            }
            return View(customerCategories);
        }

        // POST: CustomerCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Totalamount,Categoryname")] CustomerCategories customerCategories)
        {
            if (id != customerCategories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerCategories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerCategoriesExists(customerCategories.Id))
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
            return View(customerCategories);
        }

        // GET: CustomerCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerCategories = await _context.CustomerCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerCategories == null)
            {
                return NotFound();
            }

            return View(customerCategories);
        }

        // POST: CustomerCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerCategories = await _context.CustomerCategories.FindAsync(id);
            _context.CustomerCategories.Remove(customerCategories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerCategoriesExists(int id)
        {
            return _context.CustomerCategories.Any(e => e.Id == id);
        }
    }
}
