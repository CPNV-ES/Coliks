using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coliks.Models;
using Newtonsoft.Json.Linq;

namespace coliks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly ColiksContext _context;

        public APIController(ColiksContext context)
        {
            _context = context;
        }

        [HttpGet("/api/contracts/{id}")]
        public async Task<ActionResult<Contracts>> GetContract(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return await _context.Contracts.FindAsync(id);
        }

        [HttpGet("/api/names-list/{lastName}")]
        public async Task<ActionResult<List<Customers>>> GetNamesList(String lastName)
        {
            if (lastName == null)
            {
                return NotFound();
            }

            return await _context.Customers.Where(customer => customer.Lastname == lastName).OrderBy(c => c.Firstname).Include(c => c.City).ToListAsync();
        }

        [HttpGet("/api/customer-contracts/{id}")]
        public async Task<ActionResult<List<Contracts>>> GetCustomerContracts(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            return await _context.Contracts.Where(contract => contract.CustomerId == id).ToListAsync();
        }

        [HttpGet("/api/items")]
        public async Task<ActionResult<List<Items>>> GetItems(String input, String inputNumber)
        {
            if (input == null && inputNumber == null)
            {
                return NotFound();
            }

            if (input != null)
            {
                return await _context.Items.Where(i => i.Brand.Contains(input) || i.Model.Contains(input)).Include(i => i.Category).Take(50).ToListAsync();
            } else if (inputNumber != null)
            {
                return await _context.Items.Where(i => i.Itemnb.Contains(inputNumber)).Include(i => i.Category).Take(50).ToListAsync();
            } else
            {
                return NotFound();
            }
        }

        [HttpGet("/api/durations")]
        public async Task<ActionResult<List<Durations>>> GetDurations()
        {
            return await _context.Durations.ToListAsync();
        }

        [HttpGet("/api/get-price")]
        public async Task<ActionResult<Rentprices>> GetPriceItem(int? CategoryId, String ItemType, int DurationId, String CategoryCode)
        {
            var gearType = await _context.Geartypes.FirstOrDefaultAsync(gt => gt.Name == ItemType);
            if (CategoryId == null) {
                var category = await _context.Categories.Where(c => c.Code == CategoryCode).FirstOrDefaultAsync();
                CategoryId = category.Id;
            }
            return await _context.Rentprices.Where(rt => rt.CategoryId == CategoryId && rt.DurationId == DurationId && rt.GeartypeId == gearType.Id).FirstOrDefaultAsync();
        }

        [HttpPost("/api/contracts/create")]
        public async Task<ActionResult<Contracts>> PostContract(Contracts Contract)
        {
            Contract.Creationdate = DateTime.Now;
            if (DateTime.Now.Month < 4)
            {
                Contract.Plannedreturn = new DateTime(DateTime.Now.Year, 4, 1);
            } else {
                Contract.Plannedreturn = new DateTime(DateTime.Now.Year + 1, 4, 1);
            }
            _context.Contracts.Add(Contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContract), new { id = Contract.Id }, Contract);
        }
    }
}