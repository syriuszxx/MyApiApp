using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiApp.Models;

namespace MyApiApp.Controllers
{
    /// <summary>
    /// Operating on Account 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalAccountController(MyDb context) : ControllerBase
    {
        private readonly MyDb _context = context;
        
        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <returns></returns>
        // GET: api/PersonalAccount
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalAccount>>> GetPersonalAccounts()
        {
            var personalAccountResult = await _context.PersonalAccounts.ToListAsync();
            await ResponseSaver.SaveResponseAsync(personalAccountResult, "personalAccount", "get");
            return personalAccountResult;
        }

        /// <summary>
        /// get account by accountId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/PersonalAccount/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonalAccount>> GetPersonalAccount(int id)
        {
            var personalAccount = await _context.PersonalAccounts.FindAsync(id);

            if (personalAccount == null)
            {
                return NotFound();
            }

            await ResponseSaver.SaveResponseAsync(personalAccount, "personalAccount", "get");
            return personalAccount;
        }

        /// <summary>
        /// Create account.
        /// </summary>
        /// <returns>The created if found.</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Not valid customer request.</response>        /// 
        /// <response code="404">If the account not exists or is not found</response>
        // POST: api/PersonalAccount
        [HttpPost]
        public async Task<ActionResult<PersonalAccount>> PostPersonalAccount(PersonalAccount personalAccount)
        {
            // Check if CustomerId is valid (assuming 0 is not a valid CustomerId)
            if (personalAccount.CustomerId <= 0)
            {
                return BadRequest("CustomerId is required and must be greater than 0.");
            }

            // Check if the CustomerId corresponds to an existing customer


            var customer = await _context.Customers.FindAsync(personalAccount.CustomerId);

            Console.Out.WriteLine("is the customer :" + customer.ToString());
            if (customer == null)
            {
                // Return a NotFound result if the specified customer does not exist
                return NotFound($"No customer found with ID {personalAccount.CustomerId}");
            }
            
            personalAccount.Customer = customer;

            _context.PersonalAccounts.Add(personalAccount);

            await _context.SaveChangesAsync();

            await ResponseSaver.SaveResponseAsync(personalAccount, "personalAccount", "post");
            return CreatedAtAction(nameof(GetPersonalAccount), new { id = personalAccount.AccountId }, personalAccount);
        }


        /// <summary>
        /// Updates account.
        /// </summary>        
        /// <returns>No content</returns>
        /// <response code="400">Problem with account update.</response>           
        /// <response code="404">Account is not found.</response>           
        // PUT: api/PersonalAccount/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonalAccount(int id, PersonalAccount personalAccount)
        {
            if (id != personalAccount.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(personalAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PersonalAccounts.Any(e => e.AccountId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await ResponseSaver.SaveResponseAsync(personalAccount, "personalAccount", "put");
            return NoContent();
        }

        /// <summary>
        /// Delete account by ID.
        /// </summary>
        /// <param name="id">The ID of the account to delete.</param>
        /// <returns>No content</returns>
        /// <response code="204">Deletion is confirmed by status noContent</response>
        /// <response code="404">If the customer is not found</response>        
        // DELETE: api/PersonalAccount/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonalAccount(int id)
        {
            var personalAccount = await _context.PersonalAccounts.FindAsync(id);
            if (personalAccount == null)
            {
                return NotFound();
            }

            _context.PersonalAccounts.Remove(personalAccount);
            await _context.SaveChangesAsync();
            await ResponseSaver.SaveResponseAsync(personalAccount, "personalAccount", "delete");
            return NoContent();
        }
    }
}
