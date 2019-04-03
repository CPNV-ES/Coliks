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
    public class BrandsController : Controller
    {
        private readonly ColiksContext2 _context;

        public BrandsController(ColiksContext2 context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brands.ToListAsync());
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);

            var relations = await _context.Brands.Where(b => b.Id == id).Include(i => i.Items).ThenInclude(r => r.Renteditems).ThenInclude(c => c.Contract).ThenInclude(c => c.Customer).ToListAsync();

            if (brands == null)
            {
                return NotFound();
            }
            
            ViewBag.nbItems = relations[0].Items.Count();

            int? ItemsTotalCost = 0;
            int rentedItemsTotalCount = 0;
            int? rentedItemsTotalCost = 0;
            foreach (var items in relations[0].Items) // Boucler sur les items trouvé par la relation
            {
                if(items.Cost != null)
                {
                    ItemsTotalCost += items.Cost; // Cumuler le cout des items
                }
                rentedItemsTotalCount += items.Renteditems.Count(); // Nombre total de locations

                foreach(var rntItems in items.Renteditems) // Puis sur les locations (rentedItems)
                {
                    if(rntItems.Price != null)
                    {
                        rentedItemsTotalCost += rntItems.Price; // Cumuler le prix de chaque location (rentedItems)
                    }
                }
            }


            ViewBag.itemTotalCost = ItemsTotalCost;
            ViewBag.rentedTotalCount = rentedItemsTotalCount;
            ViewBag.rentedTotalCost = rentedItemsTotalCost;
            return View(brands);
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Brandname")] Brands brands)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brands);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brands);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands.FindAsync(id);
            if (brands == null)
            {
                return NotFound();
            }
            return View(brands);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brandname")] Brands brands)
        {
            if (id != brands.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brands);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandsExists(brands.Id))
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
            return View(brands);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brands == null)
            {
                return NotFound();
            }

            return View(brands);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brands = await _context.Brands.FindAsync(id);
            _context.Brands.Remove(brands);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandsExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }
    }
}
