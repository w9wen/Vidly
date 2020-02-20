using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vidly.Data;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ApplicationDbContext dbContext;
        private IMapper mapper;

        public CustomersController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        // POST /api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customerList = await this.dbContext.Customers.ToListAsync();
            var customerDtoList = mapper.Map<List<CustomerDto>>(customerList);
            return customerDtoList;
        }

        // GET /api/customers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            // var customer = await this.dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);
            var customer = await this.dbContext.Customers.FindAsync(id);

            if (customer == null)
                return NotFound();

            var customerDto = mapper.Map<CustomerDto>(customer);

            return customerDto;
        }


        // POST /api/customers
        [HttpPost]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var customer = mapper.Map<Customer>(customerDto);

            this.dbContext.Customers.Add(customer);
            await this.dbContext.SaveChangesAsync();

            customerDto.Id = customer.Id;

            // return customer;
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        // PUT /api/customers/1
        // [HttpPut("{id}")]
        [HttpPut]
        public async Task<ActionResult> UpdateCustomer(int id, CustomerDto customerDto)
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
            if (id != customerDto.Id)
                return BadRequest();

            var customer = mapper.Map<Customer>(customerDto);

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
        // [HttpDelete("{id}")]
        [HttpDelete]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            // // var customerInDb = await this.dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);

            // if (customerInDb == null)
            //     return NotFound();

            // this.dbContext.Customers.Remove(customerInDb);
            // await this.dbContext.SaveChangesAsync();
            // return Ok();

            var customer = await this.dbContext.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            this.dbContext.Customers.Remove(customer);
            await this.dbContext.SaveChangesAsync();

            return customer;

        }

        private bool CustomerExists(int id) => this.dbContext.Customers.Any(c => c.Id == id);
    }
}