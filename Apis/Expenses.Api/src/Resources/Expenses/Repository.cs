using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{
    public interface IExpensesRepository
    {
        Task<IEnumerable<Expense>> GetAsync(CancellationToken cancellationToken);
        Task DelAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(Expense expense, CancellationToken cancellationToken);
        Task<IEnumerable<Expense>> GetByCategoryAsync(string category, CancellationToken cancellationToken);
        Task<IEnumerable<Expense>> GetFromDateAsync(DateTime date, CancellationToken cancellationToken);
    }

    public class InMemoryExpensesRepository : IExpensesRepository
    {
        private int _counter;
        private readonly IDictionary<int, Expense> _buffer;
        private readonly ILogger<IExpensesRepository> _logger;

        public InMemoryExpensesRepository(ILogger<IExpensesRepository> logger)
        {
            _logger = logger;
            _counter = 0;
            _buffer = new Dictionary<int, Expense> ();
        }

        public Task AddAsync(Expense expense, CancellationToken cancellationToken)
        {
            expense.Id = _counter;

            _buffer.Add(expense.Id, expense);

            _counter++;
            
            return Task.CompletedTask;
        }

        public Task DelAsync(int id, CancellationToken cancellationToken)
        {
            if (!_buffer.Remove(id)) 
            {
                return Task.FromException(new KeyNotFoundException($"Expense with id {id} was not found"));
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Expense>> GetAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_buffer.Values.AsEnumerable());
        }

        public Task<IEnumerable<Expense>> GetByCategoryAsync(string category, CancellationToken cancellationToken)
        {
            return Task.FromResult(_buffer
                    .Values
                    .Where(e => e.Category.Equals(category, System.StringComparison.OrdinalIgnoreCase))
                    .AsEnumerable());
        }

        public Task<IEnumerable<Expense>> GetFromDateAsync(DateTime date, CancellationToken cancellationToken)
        {
            return Task.FromResult(_buffer
                    .Values
                    .Where(e => e.Timestamp >= date)
                    .AsEnumerable());
        }
    }
}