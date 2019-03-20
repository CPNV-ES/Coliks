using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coliks.Models;

namespace coliks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesAPIController : ControllerBase
    {
        private readonly ColiksContext _context;

        public PurchasesAPIController(ColiksContext context)
        {
            _context = context;
        }

        // GET: api/PurchasesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchases>>> GetPurchases()
        {
            return await _context.Purchases.ToListAsync();
        }

        // GET: api/PurchasesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchases>> GetPurchases(int id)
        {
            var purchases = await _context.Purchases.FindAsync(id);

            if (purchases == null)
            {
                return NotFound();
            }

            return purchases;
        }

        // PUT: api/PurchasesAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchases(int id, Purchases purchases)
        {
            if (id != purchases.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchases).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchasesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PurchasesAPI
        [HttpPost]
        public async Task<ActionResult<Purchases>> PostPurchases(Purchases purchases)
        {
            _context.Purchases.Add(purchases);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchases", new { id = purchases.Id }, purchases);
        }

        // DELETE: api/PurchasesAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Purchases>> DeletePurchases(int id)
        {
            var purchases = await _context.Purchases.FindAsync(id);
            if (purchases == null)
            {
                return NotFound();
            }

            _context.Purchases.Remove(purchases);
            await _context.SaveChangesAsync();

            return purchases;
        }

        private bool PurchasesExists(int id)
        {
            return _context.Purchases.Any(e => e.Id == id);
        }
    }
}
