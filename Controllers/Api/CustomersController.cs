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

        // POST /api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await this.dbContext.Customers.ToListAsync();
        }

        // GET /api/customers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            // var customer = await this.dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);
            var customer = await this.dbContext.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            return customer;
        }


        // POST /api/customers
        [HttpPost]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            this.dbContext.Customers.Add(customer);
            await this.dbContext.SaveChangesAsync();

            // return customer;
            return CreatedAtAction(nameof(GetCustomer), new {id = customer.Id}, customer);
        }

        // PUT /api/customers/1
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, Customer customer)
        {
            // if (!ModelState.IsValid)
            //     return BadRequest();

            // var customerInDb = await this.dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);
            // if (customerInDb == null)
            //     return NotFound();

            // customerInDb.Name = customer.Name;
            // customerInDb.Birthdate = customer.Birthdate;
            // customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            // customerInDb.MembershipTypeId = customer.MembershipTypeId;

            // await this.dbContext.SaveChangesAsync();
            // return Ok();
            if (id != customer.Id)
                return BadRequest();

            this.dbContext.Entry(customer).State = EntityState.Modified;

            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else throw;
            }
            return NoContent();
        }

        // DELETE /api/customers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            // // var customerInDb = await this.dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);
            
            // if (customerInDb == null)
            //     return NotFound();

            // this.dbContext.Customers.Remove(customerInDb);
            // await this.dbContext.SaveChangesAsync();
            // return Ok();

            var customer = await this.dbContext.Customers.FindAsync(id);
            if(customer == null)
             return NotFound();

             this.dbContext.Customers.Remove(customer);
             await this.dbContext.SaveChangesAsync();

             return customer;

        }

        private bool CustomerExists(int id) => this.dbContext.Customers.Any(c => c.Id == id);
    }
}