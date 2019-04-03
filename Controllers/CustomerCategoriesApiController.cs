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
    public class CustomerCategoriesApiController : ControllerBase
    {
        private readonly ColiksContext _context;

        public CustomerCategoriesApiController(ColiksContext context)
        {
            _context = context;
        }

        // GET: api/CustomerCategoriesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerCategories>>> GetCustomerCategories()
        {
            return await _context.CustomerCategories.ToListAsync();
        }

        // GET: api/CustomerCategoriesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerCategories>> GetCustomerCategories(int id)
        {
            var customerCategories = await _context.CustomerCategories.FindAsync(id);

            if (customerCategories == null)
            {
                return NotFound();
            }

            return customerCategories;
        }

        // PUT: api/CustomerCategoriesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerCategories(int id, CustomerCategories customerCategories)
        {
            if (id != customerCategories.Id)
            {
                return BadRequest();
            }

            _context.Entry(customerCategories).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerCategoriesExists(id))
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

        // POST: api/CustomerCategoriesApi
        [HttpPost]
        public async Task<ActionResult<CustomerCategories>> PostCustomerCategories(CustomerCategories customerCategories)
        {
            _context.CustomerCategories.Add(customerCategories);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerCategories", new { id = customerCategories.Id }, customerCategories);
        }

        // DELETE: api/CustomerCategoriesApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerCategories>> DeleteCustomerCategories(int id)
        {
            var customerCategories = await _context.CustomerCategories.FindAsync(id);
            if (customerCategories == null)
            {
                return NotFound();
            }

            _context.CustomerCategories.Remove(customerCategories);
            await _context.SaveChangesAsync();

            return customerCategories;
        }

        private bool CustomerCategoriesExists(int id)
        {
            return _context.CustomerCategories.Any(e => e.Id == id);
        }
    }
}
