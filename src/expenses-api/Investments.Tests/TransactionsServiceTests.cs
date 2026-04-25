using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MyAssistant.Apis.Expenses.Api.Resources.Investments;
using Xunit;

namespace Investments.Tests;

public class TransactionsServiceTests
{
    private readonly Mock<ITransactionsRepository> _repoMock = new();
    private readonly Mock<ILogger<TransactionsService>> _loggerMock = new();
    private readonly TransactionsService _sut;

    public TransactionsServiceTests()
    {
        _sut = new TransactionsService(_repoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AddTransactionAsync_DelegatesToRepositoryAndReturnsTransaction()
    {
        var transaction = new Transaction
        {
            Portfolio = new PortfolioDto { Id = "p1", Name = "My Portfolio" },
            Symbol = "AAPL",
            AssetType = "Stock",
            Type = TransactionType.Buy,
            Quantity = 10,
            Price = 150m,
            Date = DateTime.UtcNow
        };
        var stored = transaction;
        stored.Id = Guid.NewGuid().ToString();
        _repoMock.Setup(r => r.AddAsync(transaction, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(stored);

        var result = await _sut.AddTransactionAsync(transaction, CancellationToken.None);

        Assert.Same(stored, result);
        _repoMock.Verify(r => r.AddAsync(transaction, It.IsAny<CancellationToken>()), Times.Once);
    }
}
