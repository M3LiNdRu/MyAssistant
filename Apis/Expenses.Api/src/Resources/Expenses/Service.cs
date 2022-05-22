using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{
    public interface IExpensesService
    {
        Task AddAsync(Expense expense, CancellationToken cancellationToken);
        Task DelAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Expense>> GetAsync(CancellationToken cancellationToken);
    }

    public class ExpensesService : IExpensesService
    {
        private readonly IExpensesRepository _repository;
        private readonly ILogger<IExpensesService> _logger;

        public ExpensesService(ILogger<IExpensesService> logger,
        IExpensesRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public Task AddAsync(Expense expense, CancellationToken cancellationToken)
        {
            //TODO: Check if Category matches with existing, otherwise return error
            return _repository.AddAsync(expense, cancellationToken);
        }

        public Task DelAsync(int id, CancellationToken cancellationToken)
        {
            return _repository.DelAsync(id, cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetAsync(CancellationToken cancellationToken)
        {
            return _repository.GetAsync(cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetByCategoryAsync(string category, CancellationToken cancellationToken)
        {
            return _repository.GetByCategoryAsync(category, cancellationToken);
        }
    }
}