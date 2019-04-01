using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using coliks.Models;


namespace coliks.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ColiksContext _context;
        private FilterDataController filterDataController;

        public ItemsController(ColiksContext context)
        {
            filterDataController = new FilterDataController();
            _context = context;
        }

        [HttpPost]
        public ActionResult PaginateData(int pageNo, FilterItems filter)
        {
            ViewBag.categories = _context.Categories;
            // This will call the FilterData function with PageNo and filter textboxes value which we passed in our Ajax request  
            return PartialView("_ItemList", filterDataController.FilterData(pageNo, filter, _context));
        }

        public ActionResult Index()
        {
            ViewBag.categories = _context.Categories;
            // Calling FilterData function with Page number as 1 on initial load and no filter  
            return View(filterDataController.FilterData(1, new FilterItems(), _context));
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

            await _context.Items.Where(i => i.Id == id).Include(r => r.Renteditems).ThenInclude(c => c.Contract).ThenInclude(c => c.Customer).ToListAsync();

            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Itemnb,Brand,Model,Size,CategoryId,Cost,Returned,Type,Stock,Serialnumber")] Items items)
        {
            bool IsProductNameExist = _context.Items.Any
            (x => x.Itemnb == items.Itemnb && x.Id != items.Id);
            if (IsProductNameExist)
            {
                ModelState.AddModelError("Itemnb", "Ce numéro d'article existe déjà !");
            }

            if (ModelState.IsValid)
            {
                _context.Add(items);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", items.CategoryId);
            return View(items);
        }

        public JsonResult IsItemNbExist(string ItemNb, int? Id)
        {
            var validateName = _context.Items.FirstOrDefault
                                (x => x.Itemnb == ItemNb && x.Id != Id);
            if (validateName != null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", items.CategoryId);
            return View(items);
        }

        // POST: Items/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Itemnb,Brand,Model,Size,CategoryId,Cost,Returned,Type,Stock,Serialnumber")] Items items)
        {
            if(id == null) // multiple items edit
            {
                var ids = Request.Form["isSelected"]; // All selected items
                var category = Request.Form["multiple_category"];
                var stock = Request.Form["multiple_stock"];
            

                if (ids.Count > 0)
                {
                    foreach (var i in ids)
                    {
                        try
                        {
                            Items item = _context.Items.Find(Convert.ToInt32(i));

                            if (!string.IsNullOrEmpty(category))
                            {
                                Categories cat = _context.Categories.Find(Convert.ToInt32(category)); 
                                item.Category = new Categories(); // Item always have a null category, so we have to create a new category object
                                item.Category = cat; // And asign the category
                           
                            }

                            if (!string.IsNullOrEmpty(stock))
                            {
                                item.Stock = Convert.ToInt32(stock);
                            }

                            _context.Update(item);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ItemsExists(_context.Items.Find(i).Id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else // Single edit of item
            {
                if (id != items.Id)
                {
                    return NotFound();
                }

                bool IsProductNameExist = _context.Items.Any
                    (x => x.Itemnb == items.Itemnb && x.Id != items.Id);
                if (IsProductNameExist)
                {
                    ModelState.AddModelError("Itemnb", "Ce numéro d'article existe déjà !");
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
            items.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemsExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
