using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApiApp.Models;

namespace MyApiApp.Controllers
{
    /// <summary>
    /// User login data
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(MyDb context) : ControllerBase
    {
        private readonly MyDb _context = context;        

        /// <summary>
        /// All users
        /// </summary>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            await ResponseSaver.SaveResponseAsync(users, "user", "get");
            return users; 
        }


        /// <summary>
        /// Collect user data by userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await ResponseSaver.SaveResponseAsync(user, "user", "get");
            return user;
        }

        /// <summary>
        /// Creates user.
        /// </summary>        
        /// <returns>The created user.</returns>
        /// <response code="201">Returns created user</response>     
        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await ResponseSaver.SaveResponseAsync(user, "user", "post");
            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }



        /// <summary>
        /// Updates user.
        /// </summary>        
        /// <returns>No content</returns>
        /// <response code="400">Problem with user update.</response>           
        /// <response code="404">Account is not found.</response>           
        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.UserId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await ResponseSaver.SaveResponseAsync(user, "user", "put");
            return NoContent();
        }

        /// <summary>
        /// Delete customer by ID.
        /// </summary>
        /// <param name="id">The userId of the user to delete.</param>
        /// <returns>Status Ok.</returns>
        /// <response code="200">Deletion is confirmed by status Ok</response>
        /// <response code="404">If the customer is not found</response>        
        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            await ResponseSaver.SaveResponseAsync(user, "user", "delete");
            return NoContent();
        }
    }
}