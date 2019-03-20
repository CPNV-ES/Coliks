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
        public async Task<ActionResult<Rentprices>> GetPriceItem(int CategoryId, String ItemType, int DurationId)
        {
            var gearType = await _context.Geartypes.FirstOrDefaultAsync(gt => gt.Name == ItemType);
            return await _context.Rentprices.Where(rt => rt.CategoryId == CategoryId && rt.DurationId == DurationId && rt.GeartypeId == gearType.Id).FirstOrDefaultAsync();
        }
    }
}