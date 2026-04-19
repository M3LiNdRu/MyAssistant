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
using Swashbuckle.AspNetCore.Annotations;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    [Authorize]
    [ApiController]
    public class InvestmentsController : ControllerBase
    {
        private readonly IPortfoliosService _portfoliosService;
        private readonly IInvestmentsService _investmentsService;
        private readonly ILogger<InvestmentsController> _logger;

        public InvestmentsController(
            IPortfoliosService portfoliosService,
            IInvestmentsService investmentsService,
            ILogger<InvestmentsController> logger)
        {
            _portfoliosService = portfoliosService;
            _investmentsService = investmentsService;
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

        #region Investment Endpoints

        [HttpPost]
        [Route("/api/v1/investment")]
        [ValidateModelState]
        [SwaggerOperation("CreateInvestment")]
        [SwaggerResponse(statusCode: 200, type: typeof(InvestmentResponse), description: "Investment created successfully")]
        public virtual async Task<IActionResult> CreateInvestment(
            [FromBody] InvestmentRequest body,
            CancellationToken cancellationToken)
        {
            // Verify portfolio belongs to user
            var portfolio = await _portfoliosService.GetPortfolioByIdAsync(body.PortfolioId, cancellationToken);
            if (portfolio == null || portfolio.UserId != GetUserId())
            {
                return Unauthorized("Portfolio not found or does not belong to user");
            }

            var investment = new Investment
            {
                PortfolioId = body.PortfolioId,
                AssetType = body.AssetType,
                Symbol = body.Symbol,
                Quantity = body.Quantity,
                PurchasePrice = body.PurchasePrice,
                PurchaseDate = body.PurchaseDate,
                CurrentPrice = body.CurrentPrice > 0 ? body.CurrentPrice : body.PurchasePrice
            };

            await _investmentsService.AddInvestmentAsync(investment, cancellationToken);

            var response = new InvestmentResponse
            {
                Id = investment.Id,
                PortfolioId = investment.PortfolioId,
                AssetType = investment.AssetType,
                Symbol = investment.Symbol,
                Quantity = investment.Quantity,
                PurchasePrice = investment.PurchasePrice,
                PurchaseDate = investment.PurchaseDate,
                CurrentPrice = investment.CurrentPrice,
                CreatedAt = investment.CreatedAt,
                UpdatedAt = investment.UpdatedAt
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("/api/v1/portfolio/{portfolioId}/investments")]
        [ValidateModelState]
        [SwaggerOperation("GetPortfolioInvestments")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<InvestmentResponse>), description: "List of investments")]
        public virtual async Task<IActionResult> GetPortfolioInvestments(
            [FromRoute][Required] string portfolioId,
            CancellationToken cancellationToken)
        {
            // Verify portfolio belongs to user
            var portfolio = await _portfoliosService.GetPortfolioByIdAsync(portfolioId, cancellationToken);
            if (portfolio == null || portfolio.UserId != GetUserId())
            {
                return NotFound();
            }

            var investments = await _investmentsService.GetPortfolioInvestmentsAsync(portfolioId, cancellationToken);

            var responses = investments.Select(i => new InvestmentResponse
            {
                Id = i.Id,
                PortfolioId = i.PortfolioId,
                AssetType = i.AssetType,
                Symbol = i.Symbol,
                Quantity = i.Quantity,
                PurchasePrice = i.PurchasePrice,
                PurchaseDate = i.PurchaseDate,
                CurrentPrice = i.CurrentPrice,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            });

            return Ok(responses);
        }

        [HttpGet]
        [Route("/api/v1/investment/{investmentId}")]
        [ValidateModelState]
        [SwaggerOperation("GetInvestment")]
        [SwaggerResponse(statusCode: 200, type: typeof(InvestmentResponse), description: "Investment details")]
        public virtual async Task<IActionResult> GetInvestment(
            [FromRoute][Required] string investmentId,
            CancellationToken cancellationToken)
        {
            var investment = await _investmentsService.GetInvestmentByIdAsync(investmentId, cancellationToken);

            if (investment == null)
            {
                return NotFound();
            }

            // Verify portfolio belongs to user
            var portfolio = await _portfoliosService.GetPortfolioByIdAsync(investment.PortfolioId, cancellationToken);
            if (portfolio == null || portfolio.UserId != GetUserId())
            {
                return NotFound();
            }

            var response = new InvestmentResponse
            {
                Id = investment.Id,
                PortfolioId = investment.PortfolioId,
                AssetType = investment.AssetType,
                Symbol = investment.Symbol,
                Quantity = investment.Quantity,
                PurchasePrice = investment.PurchasePrice,
                PurchaseDate = investment.PurchaseDate,
                CurrentPrice = investment.CurrentPrice,
                CreatedAt = investment.CreatedAt,
                UpdatedAt = investment.UpdatedAt
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("/api/v1/investment/{investmentId}")]
        [ValidateModelState]
        [SwaggerOperation("UpdateInvestment")]
        [SwaggerResponse(statusCode: 200, type: typeof(InvestmentResponse), description: "Investment updated successfully")]
        public virtual async Task<IActionResult> UpdateInvestment(
            [FromRoute][Required] string investmentId,
            [FromBody] UpdateInvestmentRequest body,
            CancellationToken cancellationToken)
        {
            var investment = await _investmentsService.GetInvestmentByIdAsync(investmentId, cancellationToken);

            if (investment == null)
            {
                return NotFound();
            }

            // Verify portfolio belongs to user
            var portfolio = await _portfoliosService.GetPortfolioByIdAsync(investment.PortfolioId, cancellationToken);
            if (portfolio == null || portfolio.UserId != GetUserId())
            {
                return NotFound();
            }

            investment.Quantity = body.Quantity;
            investment.CurrentPrice = body.CurrentPrice;

            await _investmentsService.UpdateInvestmentAsync(investment, cancellationToken);

            var response = new InvestmentResponse
            {
                Id = investment.Id,
                PortfolioId = investment.PortfolioId,
                AssetType = investment.AssetType,
                Symbol = investment.Symbol,
                Quantity = investment.Quantity,
                PurchasePrice = investment.PurchasePrice,
                PurchaseDate = investment.PurchaseDate,
                CurrentPrice = investment.CurrentPrice,
                CreatedAt = investment.CreatedAt,
                UpdatedAt = investment.UpdatedAt
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("/api/v1/investment/{investmentId}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteInvestment")]
        [SwaggerResponse(statusCode: 200, description: "Investment deleted successfully")]
        public virtual async Task<IActionResult> DeleteInvestment(
            [FromRoute][Required] string investmentId,
            CancellationToken cancellationToken)
        {
            var investment = await _investmentsService.GetInvestmentByIdAsync(investmentId, cancellationToken);

            if (investment == null)
            {
                return NotFound();
            }

            // Verify portfolio belongs to user
            var portfolio = await _portfoliosService.GetPortfolioByIdAsync(investment.PortfolioId, cancellationToken);
            if (portfolio == null || portfolio.UserId != GetUserId())
            {
                return NotFound();
            }

            await _investmentsService.DelInvestmentAsync(investmentId, cancellationToken);
            return Ok();
        }

        #endregion

        #region Summary Endpoints

        [HttpGet]
        [Route("/api/v1/portfolio/{portfolioId}/summary")]
        [ValidateModelState]
        [SwaggerOperation("GetPortfolioSummary")]
        [SwaggerResponse(statusCode: 200, type: typeof(PortfolioSummaryResponse), description: "Portfolio summary with metrics")]
        public virtual async Task<IActionResult> GetPortfolioSummary(
            [FromRoute][Required] string portfolioId,
            CancellationToken cancellationToken)
        {
            // Verify portfolio belongs to user
            var portfolio = await _portfoliosService.GetPortfolioByIdAsync(portfolioId, cancellationToken);
            if (portfolio == null || portfolio.UserId != GetUserId())
            {
                return NotFound();
            }

            var investments = await _investmentsService.GetPortfolioInvestmentsAsync(portfolioId, cancellationToken);
            var investmentList = investments.ToList();

            var totalCost = investmentList.Sum(i => i.Quantity * i.PurchasePrice);
            var totalValue = investmentList.Sum(i => i.Quantity * i.CurrentPrice);
            var gainLoss = totalValue - totalCost;
            var gainLossPercent = totalCost > 0 ? (gainLoss / totalCost) * 100 : 0;

            // Calculate asset allocation
            var assetAllocation = investmentList
                .GroupBy(i => i.AssetType)
                .ToDictionary(
                    g => g.Key,
                    g => new AssetAllocationItem
                    {
                        Type = g.Key,
                        Value = g.Sum(i => i.Quantity * i.CurrentPrice),
                        Percentage = totalValue > 0 ? (g.Sum(i => i.Quantity * i.CurrentPrice) / totalValue) * 100 : 0
                    }
                );

            var response = new PortfolioSummaryResponse
            {
                PortfolioId = portfolioId,
                TotalInvestments = investmentList.Count,
                TotalCost = totalCost,
                TotalValue = totalValue,
                GainLoss = gainLoss,
                GainLossPercent = gainLossPercent,
                AssetAllocation = assetAllocation
            };

            return Ok(response);
        }

        #endregion
    }
}
