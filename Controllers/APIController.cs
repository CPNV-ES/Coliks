/**
* Controller for all API endpoints in the project
*/
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
    // Specify that the controller is an API controller
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly ColiksContext _context;

        public APIController(ColiksContext context)
        {
            _context = context;
        }

        // Endpoint to get a contract from the id
        [HttpGet("/api/contracts/{id}")]
        public async Task<ActionResult<Contracts>> GetContract(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return await _context.Contracts.FindAsync(id);
        }

        // Get all the customers with a last name specified
        [HttpGet("/api/names-list/{lastName}")]
        public async Task<ActionResult<List<Customers>>> GetNamesList(String lastName)
        {
            if (lastName == null)
            {
                return NotFound();
            }

            return await _context.Customers.Where(customer => customer.Lastname == lastName).OrderBy(c => c.Firstname).Include(c => c.City).ToListAsync();
        }

        // Get all the contracts for a customer
        [HttpGet("/api/customer-contracts/{id}")]
        public async Task<ActionResult<List<Contracts>>> GetCustomerContracts(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            return await _context.Contracts.Where(contract => contract.CustomerId == id).ToListAsync();
        }

        // Get all items depending on the brand/model or item number
        [HttpGet("/api/items")]
        public async Task<ActionResult<List<Items>>> GetItems(String input, String inputNumber)
        {
            // Check if both query strings are null
            if (input == null && inputNumber == null)
            {
                return NotFound();
            }

            // Check if the user typed in the brand/model input
            if (input != null)
            {
                return await _context.Items.Where(i => i.Brand.Contains(input) || i.Model.Contains(input)).Include(i => i.Category).Take(50).ToListAsync();
            // Check if the user typed in the item number input
            } else if (inputNumber != null)
            {
                return await _context.Items.Where(i => i.Itemnb.Contains(inputNumber)).Include(i => i.Category).Take(50).ToListAsync();
            } else
            {
                return NotFound();
            }
        }

        // Get all the durations
        [HttpGet("/api/durations")]
        public async Task<ActionResult<List<Durations>>> GetDurations()
        {
            return await _context.Durations.ToListAsync();
        }

        // Get all the cities with the name that contains the input sent as query string
        [HttpGet("/api/cities")]
        public async Task<ActionResult<List<Cities>>> GetCities(String input)
        {
            if (input == null) { return NotFound(); }
            return await _context.Cities.Where(c => c.Name.Contains(input)).Take(50).ToListAsync();
        }

        // Get the price of an item depending on the category and the duration
        [HttpGet("/api/get-price")]
        public async Task<ActionResult<Rentprices>> GetPriceItem(int? CategoryId, String ItemType, int DurationId, String CategoryCode)
        {
            // Get the geartype since the geartype is required in the RentPrices model
            var gearType = await _context.Geartypes.FirstOrDefaultAsync(gt => gt.Name == ItemType);
            // Check if the category id is given in the query string since this endpoint is called on duration change in the form
            if (CategoryId == null) {
                var category = await _context.Categories.Where(c => c.Code == CategoryCode).FirstOrDefaultAsync();
                CategoryId = category.Id;
            }
            // Get the price with all the information sent
            return await _context.Rentprices.Where(rt => rt.CategoryId == CategoryId && rt.DurationId == DurationId && rt.GeartypeId == gearType.Id).FirstOrDefaultAsync();
        }

        // Endpoint to create the contract
        [HttpPost("/api/contracts/create")]
        public async Task<ActionResult<Contracts>> PostContract(Contracts Contract)
        {
            Contract.Creationdate = DateTime.Now;
            // Check if it's before April, if yes, the planned return is the 1st of April of this year, if not, it's the 1st of April next year
            if (DateTime.Now.Month < 4)
            {
                Contract.Plannedreturn = new DateTime(DateTime.Now.Year, 4, 1);
            } else {
                Contract.Plannedreturn = new DateTime(DateTime.Now.Year + 1, 4, 1);
            }
            // Add the new contract and save it in DB
            _context.Contracts.Add(Contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContract), new { id = Contract.Id }, Contract);
        }

        [HttpPut("/api/customers/{id}")]
        public async Task<ActionResult<Customers>> PutCustomer(int? id, Customers customer)
        {
            if (id == null || customer == null)
            {
                return NotFound();
            }
            var customerDB = await _context.Customers.FindAsync(id);
            customerDB.Address = customer.Address;
            customerDB.CityId = customer.CityId;
            customerDB.Phone = customer.Phone;
            customerDB.Mobile = customer.Mobile;
            customerDB.Email = customer.Email;
            await _context.SaveChangesAsync();
            return customerDB;
        }

        // Change the category of an item
        [HttpPut("/api/change-item-category/{id}")]
        public async Task<ActionResult<Items>> PutItem(int? id, Categories category)
        {
            if (id == null && category == null) {
                return NotFound();
            }
            var categoryDB = await _context.Categories.Where(c => c.Code == category.Code).FirstOrDefaultAsync();
            var itemDB = await _context.Items.FindAsync(id);
            itemDB.CategoryId = categoryDB.Id;
            await _context.SaveChangesAsync();
            return itemDB;
        }

        // Endpoint to update a contract and put it in 'paid' status
        [HttpPut("/api/paid-contract/{id}")]
        public async Task<ActionResult<Contracts>> ContractPaid(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contract = await _context.Contracts.FindAsync(id);
            contract.Paidon = DateTime.Now;
            await _context.SaveChangesAsync();
            return contract;
        }

        // Endpoint to update a contract and put it in 'returned' status
        [HttpPut("/api/returned-contract/{id}")]
        public async Task<ActionResult<Contracts>> ContractReturned(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contract = await _context.Contracts.FindAsync(id);
            contract.Effectivereturn = DateTime.Now;
            await _context.SaveChangesAsync();
            return contract;
        }
    }
}