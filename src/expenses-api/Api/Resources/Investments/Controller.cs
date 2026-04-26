using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAssistant.Apis.Expenses.Api.Attributes;
using MyAssistant.Apis.Expenses.Api.FeatureFlags;
using Swashbuckle.AspNetCore.Annotations;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    [Authorize]
    [ApiController]
    public class InvestmentsController : ControllerBase
    {
        private readonly IPortfoliosService _portfoliosService;
        private readonly ITransactionsService _transactionsService;
        private readonly ILogger<InvestmentsController> _logger;

        public InvestmentsController(
            IPortfoliosService portfoliosService,
            ITransactionsService transactionsService,
            ILogger<InvestmentsController> logger)
        {
            _portfoliosService = portfoliosService;
            _transactionsService = transactionsService;
            _logger = logger;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   User.FindFirst("sub")?.Value ??
                   User.Identity.Name;
        }

        #region Portfolio Endpoints

        [HttpPost]
        [Route("/api/v1/portfolio")]
        [ValidateModelState]
        [RequireFeature(nameof(FeatureFlagSettings.EnablePortfolioCreation))]
        [SwaggerOperation("CreatePortfolio")]
        [SwaggerResponse(statusCode: 200, type: typeof(PortfolioResponse), description: "Portfolio created successfully")]
        public virtual async Task<IActionResult> CreatePortfolio(
            [FromBody] PortfolioRequest body,
            CancellationToken cancellationToken)
        {
            var portfolio = new Portfolio
            {
                UserId = GetUserId(),
                Name = body.Name,
                Description = body.Description
            };

            await _portfoliosService.AddPortfolioAsync(portfolio, cancellationToken);

            var response = new PortfolioResponse
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                Description = portfolio.Description,
                CreatedAt = portfolio.CreatedAt,
                UpdatedAt = portfolio.UpdatedAt
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("/api/v1/portfolios")]
        [ValidateModelState]
        [SwaggerOperation("GetUserPortfolios")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<PortfolioResponse>), description: "List of user portfolios")]
        public virtual async Task<IActionResult> GetUserPortfolios(CancellationToken cancellationToken)
        {
            var portfolios = await _portfoliosService.GetUserPortfoliosAsync(GetUserId(), cancellationToken);

            var responses = portfolios.Select(p => new PortfolioResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });

            return Ok(responses);
        }

        [HttpGet]
        [Route("/api/v1/portfolio/{portfolioId}")]
        [ValidateModelState]
        [SwaggerOperation("GetPortfolio")]
        [SwaggerResponse(statusCode: 200, type: typeof(PortfolioResponse), description: "Portfolio details")]
        public virtual async Task<IActionResult> GetPortfolio(
            [FromRoute][Required] string portfolioId,
            CancellationToken cancellationToken)
        {
            var portfolio = await _portfoliosService.GetPortfolioByIdAsync(portfolioId, cancellationToken);

            if (portfolio == null || portfolio.UserId != GetUserId())
            {
                return NotFound();
            }

            var response = new PortfolioResponse
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                Description = portfolio.Description,
                CreatedAt = portfolio.CreatedAt,
                UpdatedAt = portfolio.UpdatedAt
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("/api/v1/portfolio/{portfolioId}")]
        [ValidateModelState]
        [SwaggerOperation("UpdatePortfolio")]
        [SwaggerResponse(statusCode: 200, type: typeof(PortfolioResponse), description: "Portfolio updated successfully")]
        public virtual async Task<IActionResult> UpdatePortfolio(
            [FromRoute][Required] string portfolioId,
            [FromBody] PortfolioRequest body,
            CancellationToken cancellationToken)
        {
            var portfolio = await _portfoliosService.GetPortfolioByIdAsync(portfolioId, cancellationToken);

            if (portfolio == null || portfolio.UserId != GetUserId())
            {
                return NotFound();
            }

            portfolio.Name = body.Name;
            portfolio.Description = body.Description;

            await _portfoliosService.UpdatePortfolioAsync(portfolio, cancellationToken);

            var response = new PortfolioResponse
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                Description = portfolio.Description,
                CreatedAt = portfolio.CreatedAt,
                UpdatedAt = portfolio.UpdatedAt
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("/api/v1/portfolio/{portfolioId}")]
        [ValidateModelState]
        [SwaggerOperation("DeletePortfolio")]
        [SwaggerResponse(statusCode: 200, description: "Portfolio deleted successfully")]
        public virtual async Task<IActionResult> DeletePortfolio(
            [FromRoute][Required] string portfolioId,
            CancellationToken cancellationToken)
        {
            var portfolio = await _portfoliosService.GetPortfolioByIdAsync(portfolioId, cancellationToken);

            if (portfolio == null || portfolio.UserId != GetUserId())
            {
                return NotFound();
            }

            await _portfoliosService.DelPortfolioAsync(portfolioId, cancellationToken);
            return Ok();
        }

        #endregion

        #region Transaction Endpoints

        [HttpGet]
        [Route("/api/v1/transactions/recent")]
        [ValidateModelState]
        [SwaggerOperation("GetRecentTransactions")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<TransactionResponse>), description: "Last N transactions for the user")]
        public virtual async Task<IActionResult> GetRecentTransactions(
            [FromQuery] int limit = 10,
            CancellationToken cancellationToken = default)
        {
            var transactions = await _transactionsService.GetRecentTransactionsAsync(GetUserId(), limit, cancellationToken);

            var responses = transactions.Select(t => new TransactionResponse
            {
                Id = t.Id,
                Portfolio = t.Portfolio,
                Symbol = t.Symbol,
                AssetType = t.AssetType,
                Type = t.Type,
                Quantity = t.Quantity,
                Price = t.Price,
                Date = t.Date,
                Notes = t.Notes,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });

            return Ok(responses);
        }

        [HttpPost]
        [Route("/api/v1/transaction")]
        [ValidateModelState]
        [SwaggerOperation("AddTransaction")]
        [SwaggerResponse(statusCode: 200, type: typeof(TransactionResponse), description: "Transaction added successfully")]
        public virtual async Task<IActionResult> AddTransaction(
            [FromBody] TransactionRequest body,
            CancellationToken cancellationToken)
        {
            var portfolio = await _portfoliosService.GetPortfolioByIdAsync(body.PortfolioId, cancellationToken);
            if (portfolio == null || portfolio.UserId != GetUserId())
                return NotFound();

            var transaction = new Transaction
            {
                Portfolio = new PortfolioDto { Id = portfolio.Id, Name = portfolio.Name },
                Symbol = body.Symbol,
                AssetType = body.AssetType,
                Type = body.Type,
                Quantity = body.Quantity,
                Price = body.Price,
                Date = body.Date,
                Notes = body.Notes
            };

            await _transactionsService.AddTransactionAsync(transaction, cancellationToken);

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                Portfolio = transaction.Portfolio,
                Symbol = transaction.Symbol,
                AssetType = transaction.AssetType,
                Type = transaction.Type,
                Quantity = transaction.Quantity,
                Price = transaction.Price,
                Date = transaction.Date,
                Notes = transaction.Notes,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };

            return Ok(response);
        }

        #endregion

    }
}
