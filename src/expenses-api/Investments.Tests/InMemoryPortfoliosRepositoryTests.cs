using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MyAssistant.Apis.Expenses.Api.Resources.Investments;
using Xunit;

namespace Investments.Tests;

public class InMemoryPortfoliosRepositoryTests
{
    private readonly InMemoryPortfoliosRepository _sut;

    public InMemoryPortfoliosRepositoryTests()
    {
        _sut = new InMemoryPortfoliosRepository(new Mock<ILogger<InMemoryPortfoliosRepository>>().Object);
    }

    [Fact]
    public async Task AddAsync_AssignsGuidId_AndTimestamps()
    {
        var portfolio = new Portfolio { UserId = "u1", Name = "Test" };

        await _sut.AddAsync(portfolio, CancellationToken.None);

        Assert.False(string.IsNullOrEmpty(portfolio.Id));
        Assert.True(Guid.TryParse(portfolio.Id, out _));
        Assert.NotEqual(default, portfolio.CreatedAt);
        Assert.NotEqual(default, portfolio.UpdatedAt);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPortfolio_WhenExists()
    {
        var portfolio = new Portfolio { UserId = "u1", Name = "Test" };
        await _sut.AddAsync(portfolio, CancellationToken.None);

        var result = await _sut.GetByIdAsync(portfolio.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(portfolio.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        var result = await _sut.GetByIdAsync("nonexistent", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsOnlyUserPortfolios()
    {
        await _sut.AddAsync(new Portfolio { UserId = "u1", Name = "P1" }, CancellationToken.None);
        await _sut.AddAsync(new Portfolio { UserId = "u1", Name = "P2" }, CancellationToken.None);
        await _sut.AddAsync(new Portfolio { UserId = "u2", Name = "P3" }, CancellationToken.None);

        var result = (await _sut.GetByUserIdAsync("u1", CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.Equal("u1", p.UserId));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesPortfolio()
    {
        var portfolio = new Portfolio { UserId = "u1", Name = "Original" };
        await _sut.AddAsync(portfolio, CancellationToken.None);
        portfolio.Name = "Updated";

        await _sut.UpdateAsync(portfolio, CancellationToken.None);

        var result = await _sut.GetByIdAsync(portfolio.Id, CancellationToken.None);
        Assert.Equal("Updated", result.Name);
    }

    [Fact]
    public async Task DelAsync_RemovesPortfolio()
    {
        var portfolio = new Portfolio { UserId = "u1", Name = "ToDelete" };
        await _sut.AddAsync(portfolio, CancellationToken.None);

        await _sut.DelAsync(portfolio.Id, CancellationToken.None);

        var result = await _sut.GetByIdAsync(portfolio.Id, CancellationToken.None);
        Assert.Null(result);
    }

    [Fact]
    public async Task DelAsync_Throws_WhenNotFound()
    {
        await Assert.ThrowsAsync<System.Collections.Generic.KeyNotFoundException>(
            () => _sut.DelAsync("nonexistent", CancellationToken.None));
    }
}
