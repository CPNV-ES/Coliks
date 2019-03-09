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

            return await _context.Customers.Where(customer => customer.Lastname == lastName).ToListAsync();
        }

        [HttpGet("/api/customer-contracts/{id}")]
        public async Task<ActionResult<List<Contracts>>> GetCustomerContracts(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            return await _context.Contracts.Where(contract => contract.CustomerId == id).ToListAsync();
        }
    }
}