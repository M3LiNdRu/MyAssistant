using System;
using System.Threading;
using System.Threading.Tasks;
using MyAssistant.Apis.Expenses.Api.Resources.Investments;
using Xunit;

namespace Investments.Tests;

public class InMemoryTransactionsRepositoryTests
{
    private readonly InMemoryTransactionsRepository _sut = new();

    [Fact]
    public async Task AddAsync_AssignsGuidId_AndTimestamps()
    {
        var transaction = new Transaction
        {
            Portfolio = new PortfolioDto { Id = "p1", Name = "My Portfolio" },
            Symbol = "AAPL",
            AssetType = "Stock",
            Type = TransactionType.Buy,
            Quantity = 5,
            Price = 200m,
            Date = DateTime.UtcNow
        };

        var result = await _sut.AddAsync(transaction, CancellationToken.None);

        Assert.False(string.IsNullOrEmpty(result.Id));
        Assert.True(Guid.TryParse(result.Id, out _));
        Assert.NotEqual(default, result.CreatedAt);
        Assert.NotEqual(default, result.UpdatedAt);
    }

    [Fact]
    public async Task AddAsync_ReturnsSameTransactionInstance()
    {
        var transaction = new Transaction
        {
            Portfolio = new PortfolioDto { Id = "p1", Name = "My Portfolio" },
            Symbol = "TSLA",
            Type = TransactionType.Sell,
            Quantity = 2,
            Price = 300m,
            Date = DateTime.UtcNow
        };

        var result = await _sut.AddAsync(transaction, CancellationToken.None);

        Assert.Same(transaction, result);
    }

    [Fact]
    public async Task AddAsync_EachTransaction_GetsUniqueId()
    {
        var t1 = new Transaction { Portfolio = new PortfolioDto { Id = "p1", Name = "P" }, Symbol = "A", Type = TransactionType.Buy, Date = DateTime.UtcNow };
        var t2 = new Transaction { Portfolio = new PortfolioDto { Id = "p1", Name = "P" }, Symbol = "B", Type = TransactionType.Buy, Date = DateTime.UtcNow };

        await _sut.AddAsync(t1, CancellationToken.None);
        await _sut.AddAsync(t2, CancellationToken.None);

        Assert.NotEqual(t1.Id, t2.Id);
    }
}
