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
    public class ItemsController : Controller
    {
        private readonly ColiksContext _context;

        public ItemsController(ColiksContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index(string itemnbFilter, string brandFilter, string modelFilter, string sizeFilter, string stockFilter, string categoryFilter, string filter,int page = 1, string sortExpression = "Itemnb")
        {
            var qry = _context.Items.AsNoTracking().Include(i => i.Category).OrderBy(i => i.Itemnb).AsQueryable();  //  Add the pagination and an order by Itemnb who is the id of the item. Make it queryable for filtering
            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(p => 
                p.Itemnb.Contains(filter) ||
                p.Brand.Contains(filter) || 
                p.Model.Contains(filter) || 
                p.Stock.ToString().Contains(filter)
               );
            }

            if (!string.IsNullOrWhiteSpace(itemnbFilter))
            {
                qry = qry.Where(p => p.Itemnb.ToString().Contains(itemnbFilter));
            }

            if (!string.IsNullOrWhiteSpace(brandFilter))
            {
                qry = qry.Where(p => p.Brand.Contains(brandFilter));
            }

            if (!string.IsNullOrWhiteSpace(modelFilter))
            {
                qry = qry.Where(p => p.Model.Contains(modelFilter));
            }

            if (!string.IsNullOrWhiteSpace(sizeFilter))
            {
                qry = qry.Where(p => p.Size.ToString().Contains(sizeFilter));
            }

            if (!string.IsNullOrWhiteSpace(stockFilter))
            {
                qry = qry.Where(p => p.Stock.ToString().Contains(stockFilter));
            }

            if (!string.IsNullOrWhiteSpace(categoryFilter))
            {
                qry = qry.Where(p => p.Category.Description.Contains(categoryFilter));
            }



            var model = await PagingList.CreateAsync(qry, 100, page, sortExpression, "Itemnb"); // create the pagination with 100 items for a page, start at page 1 and add default sort by Itemnb

            model.RouteValue = new RouteValueDictionary
            {
                {"filter", filter },
                {"itemnbFilter", itemnbFilter },
                {"brandFilter", brandFilter },
                {"modelFilter", modelFilter },
                {"sizeFilter", sizeFilter },
                {"stockFilter", stockFilter },
                {"categoryFilter", categoryFilter },
            };

            return View(model); 
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Itemnb,Brand,Model,Size,CategoryId,Cost,Returned,Type,Stock,Serialnumber")] Items items)
        {
            if (ModelState.IsValid)
            {
                _context.Add(items);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code", items.CategoryId);
            return View(items);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items.FindAsync(id);
            if (items == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code", items.CategoryId);
            return View(items);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Itemnb,Brand,Model,Size,CategoryId,Cost,Returned,Type,Stock,Serialnumber")] Items items)
        {
            if (id != items.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(items);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemsExists(items.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Code", items.CategoryId);
            return View(items);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var items = await _context.Items.FindAsync(id);
            _context.Items.Remove(items);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemsExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
