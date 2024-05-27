using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiApp.Controllers;
using MyApiApp.Models;
using Moq;
using Xunit;

namespace MyApiApp.Tests.Controllers;

public class TransactionControllerTests
{
    private readonly Mock<DbSet<Transaction>> _mockSet;
    private readonly Mock<MyDb> _mockContext;
    private readonly TransactionController _controller;

    public TransactionControllerTests()
    {
        _mockContext = new Mock<MyDb>();
        _mockSet = new Mock<DbSet<Transaction>>();

        _mockContext.Setup(m => m.Transactions).Returns(_mockSet.Object); // Use the already created _mockSet
        _mockSet.Setup(m => m.Add(It.IsAny<Transaction>())).Verifiable();

        _controller = new TransactionController(_mockContext.Object);
    }

    [Fact]
    public async Task PostTransaction_ValidTransaction_ReturnsCreatedAtActionResult()
    {
        var transaction = new Transaction
        {
            Amount = 100,
            Date = DateTime.Now,
            AccountId = 1,
            Description = "Test Transaction",
            Type = "Credit",
            Status = "Completed"
        };

        // Mock necessary Account checks or other interactions
        var mockAccounts = new Mock<DbSet<PersonalAccount>>();
        _mockContext.Setup(m => m.PersonalAccounts).Returns(mockAccounts.Object);
        mockAccounts.Setup(m => m.FindAsync(1)).ReturnsAsync(new PersonalAccount { AccountId = 1 });

        // Mock the SaveChangesAsync method to simulate saving changes
        _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1); // Simulate one change tracked

        // Act
        var result = await _controller.PostTransaction(transaction);

        // Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.NotNull(createdAtResult);
        Assert.Equal("GetTransaction", createdAtResult.ActionName); // Ensure action name matches the actual action

        // Verify that Add was called on DbSet
        _mockSet.Verify(m => m.Add(It.IsAny<Transaction>()), Times.Once);
        // Verify that SaveChangesAsync was called
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }
}



/// <summary>
/// get all transactions
/// </summary>
/// <returns></returns>
/*          
        [Fact]
        public async Task GetTransactions_ReturnsAllTransactions()
        {
            var data = new List<Transaction>
        {
            new Transaction { TransactionId = 1, Amount = 100 },
            new Transaction { TransactionId = 2, Amount = 200 }
        }.AsQueryable();

            _mockSet.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(data.Provider);
            _mockSet.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockSet.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockSet.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var result = await _controller.GetTransactions();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnTransactions = Assert.IsType<List<Transaction>>(okResult.Value);

            Assert.Equal(2, returnTransactions.Count);
        }
    }
*/
//}
