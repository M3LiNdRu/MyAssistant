using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MyAssistant.Apis.Expenses.Api.Resources.Investments;
using Xunit;

namespace Investments.Tests;

public class PortfoliosServiceTests
{
    private readonly Mock<IPortfoliosRepository> _repoMock = new();
    private readonly Mock<ILogger<PortfoliosService>> _loggerMock = new();
    private readonly PortfoliosService _sut;

    public PortfoliosServiceTests()
    {
        _sut = new PortfoliosService(_repoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetUserPortfoliosAsync_DelegatesToRepository()
    {
        var portfolios = new List<Portfolio> { new() { Id = "1", UserId = "u1" } };
        _repoMock.Setup(r => r.GetByUserIdAsync("u1", It.IsAny<CancellationToken>()))
                 .ReturnsAsync(portfolios);

        var result = await _sut.GetUserPortfoliosAsync("u1", CancellationToken.None);

        Assert.Same(portfolios, result);
    }

    [Fact]
    public async Task GetPortfolioByIdAsync_DelegatesToRepository()
    {
        var portfolio = new Portfolio { Id = "p1" };
        _repoMock.Setup(r => r.GetByIdAsync("p1", It.IsAny<CancellationToken>()))
                 .ReturnsAsync(portfolio);

        var result = await _sut.GetPortfolioByIdAsync("p1", CancellationToken.None);

        Assert.Same(portfolio, result);
    }

    [Fact]
    public async Task AddPortfolioAsync_DelegatesToRepository()
    {
        var portfolio = new Portfolio { Name = "Test" };
        _repoMock.Setup(r => r.AddAsync(portfolio, It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        await _sut.AddPortfolioAsync(portfolio, CancellationToken.None);

        _repoMock.Verify(r => r.AddAsync(portfolio, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePortfolioAsync_DelegatesToRepository()
    {
        var portfolio = new Portfolio { Id = "p1", Name = "Updated" };
        _repoMock.Setup(r => r.UpdateAsync(portfolio, It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        await _sut.UpdatePortfolioAsync(portfolio, CancellationToken.None);

        _repoMock.Verify(r => r.UpdateAsync(portfolio, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DelPortfolioAsync_DelegatesToRepository()
    {
        _repoMock.Setup(r => r.DelAsync("p1", It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        await _sut.DelPortfolioAsync("p1", CancellationToken.None);

        _repoMock.Verify(r => r.DelAsync("p1", It.IsAny<CancellationToken>()), Times.Once);
    }
}
