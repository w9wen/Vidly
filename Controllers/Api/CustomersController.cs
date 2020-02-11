using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vidly.Data;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ApplicationDbContext dbContext;

        public CustomersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET /api/customers
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await this.dbContext.Customers.ToListAsync();
        }

        // GET /api/customers/1
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await this.dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);
            if (customer == null)
                return NotFound();

            return customer;
        }
    }
}