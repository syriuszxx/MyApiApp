using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApiApp.Models;

namespace MyApiApp.Controllers
{

    /// <summary>
    /// Transaction controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Consumes("application/json")]
    [Produces("application/json")]
    [FormatFilter]
    public class TransactionController(MyDb context) : ControllerBase
    {
        private readonly MyDb _context = context;       

        /// <summary>
        /// Retrieves all transactions.
        /// </summary>
        /// <returns>A list of transactions.</returns>
        // GET: api/Transaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            var transactions = await _context.Transactions.ToListAsync();
            await ResponseSaver.SaveResponseAsync(transactions, "transaction", "get");
            return transactions;
        }

        /// <summary>
        /// Retrieves a specific transaction by ID.
        /// </summary>
        /// <param name="id">The ID of the transaction to retrieve.</param>
        /// <returns>The requested transaction if found.</returns>
        /// <response code="200">Returns the requested transaction</response>
        /// <response code="404">If the transaction is not found</response>
        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound($"No transactions found using this ID");
            }
            
            await _context.SaveChangesAsync();
            await ResponseSaver.SaveResponseAsync(transaction, "transaction", "get");
            return transaction;
        }

        /// <summary>
        /// Retrieves a specific transaction by accountId.
        /// </summary>
        /// <param name="accountId">The accountId of the transaction to retrieve.</param>
        /// <returns>The requested transaction if found.</returns>
        /// <response code="200">Returns the requested transaction</response>
        /// <response code="404">If the transaction is not found, account not exists, or no transactions on account</response>
        // GET: api/Transaction/ByAccount/5
        [HttpGet("ByAccount/{accountId}")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByAccountId(int accountId)
        {
            var transactions = await _context.Transactions
                                             .Where(t => t.AccountId == accountId)
                                             .ToListAsync();

            if (transactions == null || !transactions.Any())
            {
                return NotFound($"No transactions found for account ID {accountId}");
            }
            await ResponseSaver.SaveResponseAsync(transactions, "transaction", "get");
            return transactions;
        }

        /// <summary>
        /// Retrieves a specific transaction by accountId.
        /// </summary>
        /// <param name="accountId">The accountId of the transaction to retrieve.</param>
        /// <returns>The requested transaction if found.</returns>
        /// <response code="200">Returns the requested transaction</response>
        /// <response code="404">If the transaction is not found, account not exists, or no transactions on account</response>
        // GET: api/Transaction/ByAccount/5

        // GET: api/Transaction/ByDate/2024-05-22
        [HttpGet("ByDate/{date}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByDate(DateTime date)
        {
            var transactions = await _context.Transactions
                                             .Where(t => t.Date.Date == date.Date)
                                             .ToListAsync();

            if (transactions == null || !transactions.Any())
            {
                return NotFound($"No transactions found on {date:yyyy-MM-dd}");
            }

            await ResponseSaver.SaveResponseAsync(transactions, "transaction", "get");
            return transactions;
        }


        /// <summary>
        /// Create transaction.
        /// </summary>
        /// <returns>The requested transaction if found.</returns>
        /// <response code="201">Created</response>
        /// <response code="404">If the account not exists or is not found</response>
        // POST: api/Transaction
        [HttpPost]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            
            if (transaction.AccountId <= 0 || !_context.Accounts.Any(a => a.AccountId == transaction.AccountId))
            {
                return BadRequest("Invalid AccountId or Account does not exist.");
            }
            
            transaction.Account = await _context.Accounts.FindAsync(transaction.AccountId);

            if (transaction.Account == null)
            {
                return NotFound($"No account found with ID {transaction.AccountId}");
            }
            transaction.Account.UpdateBalance(transaction.Amount);
            transaction.Account.UpdateAvailableBalance(transaction.Amount);


            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            await ResponseSaver.SaveResponseAsync(transaction, "transaction", "post");
            return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);
        }
    }
}