using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyAssistant.Apis.Expenses.Api.Resources.Investments;
using Xunit;

namespace Investments.Tests;

public class InvestmentsControllerTests
{
    private const string UserId = "user-123";

    private readonly Mock<IPortfoliosService> _portfoliosServiceMock = new();
    private readonly Mock<ITransactionsService> _transactionsServiceMock = new();
    private readonly InvestmentsController _sut;

    public InvestmentsControllerTests()
    {
        _sut = new InvestmentsController(
            _portfoliosServiceMock.Object,
            _transactionsServiceMock.Object,
            new Mock<ILogger<InvestmentsController>>().Object);

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, UserId)
                ]))
            }
        };
    }

    #region Portfolio Tests

    [Fact]
    public async Task CreatePortfolio_ReturnsOk_WithPortfolioResponse()
    {
        var request = new PortfolioRequest { Name = "My Portfolio", Description = "Test" };
        _portfoliosServiceMock.Setup(s => s.AddPortfolioAsync(It.IsAny<Portfolio>(), It.IsAny<CancellationToken>()))
                              .Returns(Task.CompletedTask);

        var result = await _sut.CreatePortfolio(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<PortfolioResponse>(ok.Value);
        Assert.Equal("My Portfolio", response.Name);
        Assert.Equal("Test", response.Description);
    }

    [Fact]
    public async Task GetUserPortfolios_ReturnsOk_WithList()
    {
        var portfolios = new List<Portfolio>
        {
            new() { Id = "p1", UserId = UserId, Name = "P1" },
            new() { Id = "p2", UserId = UserId, Name = "P2" }
        };
        _portfoliosServiceMock.Setup(s => s.GetUserPortfoliosAsync(UserId, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(portfolios);

        var result = await _sut.GetUserPortfolios(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsAssignableFrom<IEnumerable<PortfolioResponse>>(ok.Value);
        Assert.Equal(2, System.Linq.Enumerable.Count(response));
    }

    [Fact]
    public async Task GetPortfolio_ReturnsOk_WhenFoundAndOwned()
    {
        var portfolio = new Portfolio { Id = "p1", UserId = UserId, Name = "P1" };
        _portfoliosServiceMock.Setup(s => s.GetPortfolioByIdAsync("p1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(portfolio);

        var result = await _sut.GetPortfolio("p1", CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<PortfolioResponse>(ok.Value);
        Assert.Equal("p1", response.Id);
    }

    [Fact]
    public async Task GetPortfolio_ReturnsNotFound_WhenPortfolioNotFound()
    {
        _portfoliosServiceMock.Setup(s => s.GetPortfolioByIdAsync("p1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Portfolio?)null!);

        var result = await _sut.GetPortfolio("p1", CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetPortfolio_ReturnsNotFound_WhenOwnedByAnotherUser()
    {
        var portfolio = new Portfolio { Id = "p1", UserId = "other-user", Name = "P1" };
        _portfoliosServiceMock.Setup(s => s.GetPortfolioByIdAsync("p1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(portfolio);

        var result = await _sut.GetPortfolio("p1", CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdatePortfolio_ReturnsOk_WhenFoundAndOwned()
    {
        var portfolio = new Portfolio { Id = "p1", UserId = UserId, Name = "Old" };
        _portfoliosServiceMock.Setup(s => s.GetPortfolioByIdAsync("p1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(portfolio);
        _portfoliosServiceMock.Setup(s => s.UpdatePortfolioAsync(portfolio, It.IsAny<CancellationToken>()))
                              .Returns(Task.CompletedTask);

        var result = await _sut.UpdatePortfolio("p1", new PortfolioRequest { Name = "New" }, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<PortfolioResponse>(ok.Value);
        Assert.Equal("New", response.Name);
    }

    [Fact]
    public async Task UpdatePortfolio_ReturnsNotFound_WhenPortfolioNotFound()
    {
        _portfoliosServiceMock.Setup(s => s.GetPortfolioByIdAsync("p1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Portfolio?)null!);

        var result = await _sut.UpdatePortfolio("p1", new PortfolioRequest { Name = "X" }, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeletePortfolio_ReturnsOk_WhenFoundAndOwned()
    {
        var portfolio = new Portfolio { Id = "p1", UserId = UserId };
        _portfoliosServiceMock.Setup(s => s.GetPortfolioByIdAsync("p1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(portfolio);
        _portfoliosServiceMock.Setup(s => s.DelPortfolioAsync("p1", It.IsAny<CancellationToken>()))
                              .Returns(Task.CompletedTask);

        var result = await _sut.DeletePortfolio("p1", CancellationToken.None);

        Assert.IsType<OkResult>(result);
        _portfoliosServiceMock.Verify(s => s.DelPortfolioAsync("p1", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeletePortfolio_ReturnsNotFound_WhenPortfolioNotFound()
    {
        _portfoliosServiceMock.Setup(s => s.GetPortfolioByIdAsync("p1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Portfolio?)null!);

        var result = await _sut.DeletePortfolio("p1", CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    #endregion

    #region Transaction Tests

    [Fact]
    public async Task AddTransaction_ReturnsOk_WithTransactionResponse()
    {
        var portfolio = new Portfolio { Id = "p1", UserId = UserId, Name = "My Portfolio" };
        _portfoliosServiceMock.Setup(s => s.GetPortfolioByIdAsync("p1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(portfolio);

        var stored = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Portfolio = new PortfolioDto { Id = "p1", Name = "My Portfolio" },
            Symbol = "AAPL",
            AssetType = "Stock",
            Type = TransactionType.Buy,
            Quantity = 10,
            Price = 150m,
            Date = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _transactionsServiceMock.Setup(s => s.AddTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(stored);

        var request = new TransactionRequest
        {
            PortfolioId = "p1",
            Symbol = "AAPL",
            AssetType = "Stock",
            Type = TransactionType.Buy,
            Quantity = 10,
            Price = 150m,
            Date = DateTime.UtcNow
        };

        var result = await _sut.AddTransaction(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<TransactionResponse>(ok.Value);
        Assert.Equal("AAPL", response.Symbol);
        Assert.Equal(TransactionType.Buy, response.Type);
        Assert.Equal(10, response.Quantity);
        Assert.Equal("p1", response.Portfolio.Id);
        Assert.Equal("My Portfolio", response.Portfolio.Name);
    }

    [Fact]
    public async Task AddTransaction_ReturnsNotFound_WhenPortfolioNotFound()
    {
        _portfoliosServiceMock.Setup(s => s.GetPortfolioByIdAsync("p1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Portfolio?)null!);

        var request = new TransactionRequest { PortfolioId = "p1", Symbol = "X", Type = TransactionType.Buy, Date = DateTime.UtcNow };

        var result = await _sut.AddTransaction(request, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
        _transactionsServiceMock.Verify(s => s.AddTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AddTransaction_ReturnsNotFound_WhenPortfolioOwnedByAnotherUser()
    {
        var portfolio = new Portfolio { Id = "p1", UserId = "other-user", Name = "P" };
        _portfoliosServiceMock.Setup(s => s.GetPortfolioByIdAsync("p1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(portfolio);

        var request = new TransactionRequest { PortfolioId = "p1", Symbol = "X", Type = TransactionType.Buy, Date = DateTime.UtcNow };

        var result = await _sut.AddTransaction(request, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
        _transactionsServiceMock.Verify(s => s.AddTransactionAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion
}
