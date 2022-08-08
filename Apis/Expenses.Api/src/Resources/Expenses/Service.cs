using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{
    public interface IExpensesService
    {
        Task AddAsync(Expense expense, CancellationToken cancellationToken);
        Task DelAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<Expense>> GetAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Expense>> GetFromDateAsync(DateTime date, CancellationToken cancellationToken);
    }

    public class ExpensesService : IExpensesService
    {
        private readonly IExpensesRepository _repository;
        private readonly ITagsRepository _tagsRepository;
        private readonly ILogger<IExpensesService> _logger;

        public ExpensesService(ILogger<IExpensesService> logger,
        IExpensesRepository repository,
        ITagsRepository tagsRepository)
        {
            _logger = logger;
            _repository = repository;
            _tagsRepository = tagsRepository;
        }

        public async Task AddAsync(Expense expense, CancellationToken cancellationToken)
        {
            //TODO: Check if Category matches with existing, otherwise return error

            await InsertNewTags(expense, cancellationToken);
            await _repository.AddAsync(expense, cancellationToken);
        }

        public Task DelAsync(string id, CancellationToken cancellationToken)
        {
            return _repository.DelAsync(id, cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetAsync(CancellationToken cancellationToken)
        {
            return _repository.GetAsync(cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetFromDateAsync(DateTime date, CancellationToken cancellationToken)
        {
            return _repository.GetByMonthAsync(date, cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetByCategoryAsync(string category, CancellationToken cancellationToken)
        {
            return _repository.GetByCategoryAsync(category, cancellationToken);
        }

        private async Task InsertNewTags(Expense expense, CancellationToken cancellationToken)
        {
            var tags = await _tagsRepository.GetAsync(cancellationToken);
            var newTagNames = expense.Tags
                .Where(t => !tags.Any(t2 => t2.Name == t))
                .Select(t => new Tag { Name = t });
            
            if (newTagNames.Any())
            {
                await _tagsRepository.AppendTagsAsync(newTagNames, cancellationToken);
            }
        }
    }
}