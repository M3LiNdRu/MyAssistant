using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.MongoDb;
using Library.MongoDb.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    public interface IPortfoliosRepository
    {
        Task AddAsync(Portfolio portfolio, CancellationToken cancellationToken);
        Task UpdateAsync(Portfolio portfolio, CancellationToken cancellationToken);
        Task DelAsync(string portfolioId, CancellationToken cancellationToken);
        Task<Portfolio> GetByIdAsync(string portfolioId, CancellationToken cancellationToken);
        Task<IEnumerable<Portfolio>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
    }

    public interface IInvestmentsRepository
    {
        Task AddAsync(Investment investment, CancellationToken cancellationToken);
        Task UpdateAsync(Investment investment, CancellationToken cancellationToken);
        Task DelAsync(string investmentId, CancellationToken cancellationToken);
        Task<Investment> GetByIdAsync(string investmentId, CancellationToken cancellationToken);
        Task<IEnumerable<Investment>> GetByPortfolioIdAsync(string portfolioId, CancellationToken cancellationToken);
    }

    public class InMemoryPortfoliosRepository : IPortfoliosRepository
    {
        private int _counter;
        private readonly IDictionary<string, Portfolio> _buffer;
        private readonly ILogger<InMemoryPortfoliosRepository> _logger;

        public InMemoryPortfoliosRepository(ILogger<InMemoryPortfoliosRepository> logger)
        {
            _logger = logger;
            _buffer = new Dictionary<string, Portfolio>();
            _counter = 0;
        }

        public Task AddAsync(Portfolio portfolio, CancellationToken cancellationToken)
        {
            portfolio.Id = _counter.ToString();
            portfolio.CreatedAt = DateTime.UtcNow;
            portfolio.UpdatedAt = DateTime.UtcNow;
            _buffer.Add(portfolio.Id, portfolio);
            _counter++;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Portfolio portfolio, CancellationToken cancellationToken)
        {
            if (!_buffer.ContainsKey(portfolio.Id))
            {
                return Task.FromException(new KeyNotFoundException($"Portfolio {portfolio.Id} not found"));
            }
            portfolio.UpdatedAt = DateTime.UtcNow;
            _buffer[portfolio.Id] = portfolio;
            return Task.CompletedTask;
        }

        public Task DelAsync(string portfolioId, CancellationToken cancellationToken)
        {
            if (!_buffer.Remove(portfolioId))
            {
                return Task.FromException(new KeyNotFoundException($"Portfolio {portfolioId} not found"));
            }
            return Task.CompletedTask;
        }

        public Task<Portfolio> GetByIdAsync(string portfolioId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_buffer.ContainsKey(portfolioId) ? _buffer[portfolioId] : null);
        }

        public Task<IEnumerable<Portfolio>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_buffer.Values.Where(p => p.UserId == userId).AsEnumerable());
        }
    }

    public class InMemoryInvestmentsRepository : IInvestmentsRepository
    {
        private int _counter;
        private readonly IDictionary<string, Investment> _buffer;
        private readonly ILogger<InMemoryInvestmentsRepository> _logger;

        public InMemoryInvestmentsRepository(ILogger<InMemoryInvestmentsRepository> logger)
        {
            _logger = logger;
            _buffer = new Dictionary<string, Investment>();
            _counter = 0;
        }

        public Task AddAsync(Investment investment, CancellationToken cancellationToken)
        {
            investment.Id = _counter.ToString();
            investment.CreatedAt = DateTime.UtcNow;
            investment.UpdatedAt = DateTime.UtcNow;
            _buffer.Add(investment.Id, investment);
            _counter++;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Investment investment, CancellationToken cancellationToken)
        {
            if (!_buffer.ContainsKey(investment.Id))
            {
                return Task.FromException(new KeyNotFoundException($"Investment {investment.Id} not found"));
            }
            investment.UpdatedAt = DateTime.UtcNow;
            _buffer[investment.Id] = investment;
            return Task.CompletedTask;
        }

        public Task DelAsync(string investmentId, CancellationToken cancellationToken)
        {
            if (!_buffer.Remove(investmentId))
            {
                return Task.FromException(new KeyNotFoundException($"Investment {investmentId} not found"));
            }
            return Task.CompletedTask;
        }

        public Task<Investment> GetByIdAsync(string investmentId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_buffer.ContainsKey(investmentId) ? _buffer[investmentId] : null);
        }

        public Task<IEnumerable<Investment>> GetByPortfolioIdAsync(string portfolioId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_buffer.Values.Where(i => i.PortfolioId == portfolioId).AsEnumerable());
        }
    }

    public class MongoDbPortfoliosRepository : DataStore<Portfolio>, IPortfoliosRepository
    {
        public MongoDbPortfoliosRepository(IOptions<DbConfigurationSettings> options) : base(options, "Portfolios")
        {
        }

        public Task AddAsync(Portfolio portfolio, CancellationToken cancellationToken)
        {
            portfolio.CreatedAt = DateTime.UtcNow;
            portfolio.UpdatedAt = DateTime.UtcNow;
            return base.InsertAsync(portfolio, cancellationToken);
        }

        public Task UpdateAsync(Portfolio portfolio, CancellationToken cancellationToken)
        {
            portfolio.UpdatedAt = DateTime.UtcNow;
            return base.UpdateOneAsync(portfolio, cancellationToken);
        }

        public Task DelAsync(string portfolioId, CancellationToken cancellationToken)
        {
            return base.DeleteAsync(p => p.Id == portfolioId, cancellationToken);
        }

        public Task<Portfolio> GetByIdAsync(string portfolioId, CancellationToken cancellationToken)
        {
            return base.FindOneAsync(p => p.Id == portfolioId, cancellationToken);
        }

        public Task<IEnumerable<Portfolio>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return base.FindAllAsync(p => p.UserId == userId, cancellationToken);
        }
    }

    public class MongoDbInvestmentsRepository : DataStore<Investment>, IInvestmentsRepository
    {
        public MongoDbInvestmentsRepository(IOptions<DbConfigurationSettings> options) : base(options, "Investments")
        {
        }

        public Task AddAsync(Investment investment, CancellationToken cancellationToken)
        {
            investment.CreatedAt = DateTime.UtcNow;
            investment.UpdatedAt = DateTime.UtcNow;
            return base.InsertAsync(investment, cancellationToken);
        }

        public Task UpdateAsync(Investment investment, CancellationToken cancellationToken)
        {
            investment.UpdatedAt = DateTime.UtcNow;
            return base.UpdateOneAsync(investment, cancellationToken);
        }

        public Task DelAsync(string investmentId, CancellationToken cancellationToken)
        {
            return base.DeleteAsync(i => i.Id == investmentId, cancellationToken);
        }

        public Task<Investment> GetByIdAsync(string investmentId, CancellationToken cancellationToken)
        {
            return base.FindOneAsync(i => i.Id == investmentId, cancellationToken);
        }

        public Task<IEnumerable<Investment>> GetByPortfolioIdAsync(string portfolioId, CancellationToken cancellationToken)
        {
            return base.FindAllAsync(i => i.PortfolioId == portfolioId, cancellationToken);
        }
    }
}
