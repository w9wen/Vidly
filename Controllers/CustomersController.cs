using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vidly.Data;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext dbContext;

        public CustomersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override void Dispose(bool disposing)
        {
            this.dbContext.Dispose();
        }

        public async Task<IActionResult> Index()
        {
            // var customers = GetCustomers();
            var customers = await this.dbContext.Customers.Include(c => c.MembershipType).ToListAsync();
            return View(customers);
        }

        public async Task<IActionResult> Detail(int id)
        {
            // var customer = this.dbContext.Customers.SingleOrDefault(c => c.Id == id);
            var customer = await this.dbContext.Customers.Include(c => c.MembershipType).SingleOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        private IEnumerable<Customer> GetCustomers()
        {
            return new List<Customer>()
            {
                new Customer(){ Id = 1, Name = "John Smith"},
                new Customer(){ Id = 2, Name = "Mary William"},
            };
        }
    }
}