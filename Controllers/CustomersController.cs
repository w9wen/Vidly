using System.Collections.Generic;
using System.Linq;
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

        public IActionResult Index()
        {
            // var customers = GetCustomers();
            var customers = this.dbContext.Customers.Include(c => c.MembershipType).ToList();
            return View(customers);
        }

        public IActionResult Detail(int id)
        {
            var customer = this.dbContext.Customers.SingleOrDefault(c => c.Id == id);

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