using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    public interface IPortfoliosService
    {
        Task<IEnumerable<Portfolio>> GetUserPortfoliosAsync(string userId, CancellationToken cancellationToken);
        Task<Portfolio> GetPortfolioByIdAsync(string portfolioId, CancellationToken cancellationToken);
        Task AddPortfolioAsync(Portfolio portfolio, CancellationToken cancellationToken);
        Task UpdatePortfolioAsync(Portfolio portfolio, CancellationToken cancellationToken);
        Task DelPortfolioAsync(string portfolioId, CancellationToken cancellationToken);
    }

    public interface IInvestmentsService
    {
        Task<IEnumerable<Investment>> GetPortfolioInvestmentsAsync(string portfolioId, CancellationToken cancellationToken);
        Task<Investment> GetInvestmentByIdAsync(string investmentId, CancellationToken cancellationToken);
        Task AddInvestmentAsync(Investment investment, CancellationToken cancellationToken);
        Task UpdateInvestmentAsync(Investment investment, CancellationToken cancellationToken);
        Task DelInvestmentAsync(string investmentId, CancellationToken cancellationToken);
    }

    public class PortfoliosService : IPortfoliosService
    {
        private readonly IPortfoliosRepository _repository;
        private readonly ILogger<PortfoliosService> _logger;

        public PortfoliosService(IPortfoliosRepository repository, ILogger<PortfoliosService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task<IEnumerable<Portfolio>> GetUserPortfoliosAsync(string userId, CancellationToken cancellationToken)
        {
            return _repository.GetByUserIdAsync(userId, cancellationToken);
        }

        public Task<Portfolio> GetPortfolioByIdAsync(string portfolioId, CancellationToken cancellationToken)
        {
            return _repository.GetByIdAsync(portfolioId, cancellationToken);
        }

        public Task AddPortfolioAsync(Portfolio portfolio, CancellationToken cancellationToken)
        {
            return _repository.AddAsync(portfolio, cancellationToken);
        }

        public Task UpdatePortfolioAsync(Portfolio portfolio, CancellationToken cancellationToken)
        {
            return _repository.UpdateAsync(portfolio, cancellationToken);
        }

        public Task DelPortfolioAsync(string portfolioId, CancellationToken cancellationToken)
        {
            return _repository.DelAsync(portfolioId, cancellationToken);
        }
    }

    public interface ITransactionsService
    {
        Task<Transaction> AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
        Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(string userId, int limit, CancellationToken cancellationToken);
    }

    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _repository;
        private readonly ILogger<TransactionsService> _logger;

        public TransactionsService(ITransactionsRepository repository, ILogger<TransactionsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task<Transaction> AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            return _repository.AddAsync(transaction, cancellationToken);
        }

        public Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(string userId, int limit, CancellationToken cancellationToken)
        {
            return _repository.GetRecentByUserIdAsync(userId, limit, cancellationToken);
        }
    }

    public class InvestmentsService : IInvestmentsService
    {
        private readonly IInvestmentsRepository _repository;
        private readonly ILogger<InvestmentsService> _logger;

        public InvestmentsService(IInvestmentsRepository repository, ILogger<InvestmentsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task<IEnumerable<Investment>> GetPortfolioInvestmentsAsync(string portfolioId, CancellationToken cancellationToken)
        {
            return _repository.GetByPortfolioIdAsync(portfolioId, cancellationToken);
        }

        public Task<Investment> GetInvestmentByIdAsync(string investmentId, CancellationToken cancellationToken)
        {
            return _repository.GetByIdAsync(investmentId, cancellationToken);
        }

        public Task AddInvestmentAsync(Investment investment, CancellationToken cancellationToken)
        {
            return _repository.AddAsync(investment, cancellationToken);
        }

        public Task UpdateInvestmentAsync(Investment investment, CancellationToken cancellationToken)
        {
            return _repository.UpdateAsync(investment, cancellationToken);
        }

        public Task DelInvestmentAsync(string investmentId, CancellationToken cancellationToken)
        {
            return _repository.DelAsync(investmentId, cancellationToken);
        }
    }
}
