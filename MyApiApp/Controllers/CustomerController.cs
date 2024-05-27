using Castle.Core.Resource;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiApp.Models;

namespace MyApiApp.Controllers
{
    /// <summary>
    /// Customer controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Consumes("application/json")]
    [Produces("application/json")]
    [FormatFilter]
    public class CustomerController(MyDb context) : ControllerBase
    {
        private readonly MyDb _context=context;

        /// <summary>
        /// Receives all customers.
        /// </summary>
        /// <returns>All customers</returns>
        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {

            var customers = await _context.Customers.ToListAsync();
            await ResponseSaver.SaveResponseAsync(customers, "customer", "get");
            return customers;
        }

        /// <summary>
        /// Retrieves a specific customer by ID.
        /// </summary>
        /// <param name="id">The customerId to retrieve.</param>
        /// <returns>The requested customer if found.</returns>
        /// <response code="200">Returns the requested customer</response>
        /// <response code="404">If the customer is not found</response>        
        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await ResponseSaver.SaveResponseAsync(customer, "customer", "get");
            return customer;
        }

        /// <summary>
        /// Creates customer.
        /// </summary>        
        /// <returns>The created customer.</returns>
        /// <response code="201">Returns created customer</response>       
        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {

            if (customer.User != null)
            {                
                _context.Users.Add(customer.User);
                await _context.SaveChangesAsync();

                customer.UserId = customer.User.UserId;
            }

            _context.Customers.Add(customer);
            var customerResult =  await _context.SaveChangesAsync();
            await ResponseSaver.SaveResponseAsync(customer, "customer", "post");
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }


        /// <summary>
        /// Creates customer.
        /// </summary>        
        /// <returns>The created customer.</returns>
        /// <response code="204">No content</response>       
        /// <response code="404">If the customer is not found</response>           
        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {            
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            _context.Entry(customer).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(e => e.CustomerId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await ResponseSaver.SaveResponseAsync(StatusCode(204), "customer", "put");
            return NoContent();
        }

        /// <summary>
        /// Delete customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>Status Ok.</returns>
        /// <response code="204">Deletion is confirmed by status noContent</response>
        /// <response code="404">If the customer is not found</response>        
        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            await ResponseSaver.SaveResponseAsync(StatusCode(204), "customer", "delete");
            return NoContent();
        }
    }
}