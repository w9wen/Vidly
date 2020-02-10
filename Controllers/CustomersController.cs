using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vidly.Data;
using Vidly.Models;
using Vidly.ViewModels;

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


        public async Task<IActionResult> New()
        {
            var membershipTypes = await dbContext.MembershipTypes.ToListAsync();
            var newCustomer = new CustomerFormViewModel()
            {
                Customer = new Customer(),
                MembershipTypes = membershipTypes
            };
            return View("CustomerForm", newCustomer);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel()
                {
                    Customer = customer,
                    MembershipTypes = await this.dbContext.MembershipTypes.ToListAsync(),
                };

                return View("CustomerForm", viewModel);
            }

            if (customer.Id == 0)
            {
                await this.dbContext.Customers.AddAsync(customer);
            }
            else
            {
                var customerInDb = await this.dbContext.Customers.SingleAsync(c => c.Id == customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            }
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Customers");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);
            if (customer == null)
                return NotFound();

            var newCustomer = new CustomerFormViewModel()
            {
                Customer = customer,
                MembershipTypes = await dbContext.MembershipTypes.ToListAsync()
            };
            return View("CustomerForm", newCustomer);
        }

        // private IEnumerable<Customer> GetCustomers()
        // {
        //     return new List<Customer>()
        //     {
        //         new Customer(){ Id = 1, Name = "John Smith"},
        //         new Customer(){ Id = 2, Name = "Mary William"},
        //     };
        // }
    }
}