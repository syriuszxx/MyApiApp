using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiApp.Models;

namespace MyApiApp.Controllers
{
    /// <summary>
    /// address controller
    /// </summary>
    /// <param name="context"></param>
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Consumes("application/json")]
    [Produces("application/json")]
    [FormatFilter]
    public class AddressController(MyDb context) : ControllerBase
    {
        private readonly MyDb _context = context;

        /// <summary>
        /// get address
        /// </summary>
        /// <returns>200</returns>
        // GET: api/Address
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            var addresses = await _context.Addresses.ToListAsync();
            await ResponseSaver.SaveResponseAsync(addresses, "address", "get");
            return addresses;
        }

        /// <summary>
        /// get address by id
        /// </summary>
        /// <param name="id"></param>        
        /// <response code="404">If the customer is not found</response>   
        /// <returns>200</returns>
        // GET: api/Address/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            await ResponseSaver.SaveResponseAsync(address, "address", "get");
            return address;
        }

        /// <summary>
        /// create address
        /// </summary>
        /// <param name="address"></param>
        /// <returns>201 created</returns>
        // POST: api/Address
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            await ResponseSaver.SaveResponseAsync(address, "address", "post");
            return CreatedAtAction(nameof(GetAddress), new { id = address.AddressId }, address);
        }
        /// <summary>
        /// modify address
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <response code="400">not valid request</response>
        /// <response code="404">If the customer is not found</response>   
        /// <returns>204 noContent</returns>
        // PUT: api/Address/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await ResponseSaver.SaveResponseAsync(address, "address", "put");
            return NoContent();
        }
        /// <summary>
        /// delete address
        /// </summary>
        /// <param name="id"></param>
        /// <response code="404">If the customer is not found</response>   
        /// <returns>204 noContent</returns>

        // DELETE: api/Address/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            await ResponseSaver.SaveResponseAsync(address, "address", "delete");
            return NoContent();
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.AddressId == id);
        }
    }
}
