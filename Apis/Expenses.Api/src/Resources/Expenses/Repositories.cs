using Library.MongoDb;
using Library.MongoDb.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{
    public interface IExpensesRepository
    {
        Task<IEnumerable<Expense>> GetAsync(CancellationToken cancellationToken);
        Task DelAsync(string id, CancellationToken cancellationToken);
        Task AddAsync(Expense expense, CancellationToken cancellationToken);
        Task<IEnumerable<Expense>> GetByCategoryAsync(string category, CancellationToken cancellationToken);
        Task<IEnumerable<Expense>> GetByMonthAsync(DateTime date, CancellationToken cancellationToken);
    }

    public class InMemoryExpensesRepository : IExpensesRepository
    {
        private int _counter;
        private readonly IDictionary<string, Expense> _buffer;
        private readonly ILogger<IExpensesRepository> _logger;

        public InMemoryExpensesRepository(ILogger<IExpensesRepository> logger)
        {
            _logger = logger;
            _counter = 0;
            _buffer = new Dictionary<string, Expense> ();
        }

        public Task AddAsync(Expense expense, CancellationToken cancellationToken)
        {
            expense.Id = _counter.ToString();

            _buffer.Add(expense.Id, expense);

            _counter++;
            
            return Task.CompletedTask;
        }

        public Task DelAsync(string id, CancellationToken cancellationToken)
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

        public Task<IEnumerable<Expense>> GetByMonthAsync(DateTime date, CancellationToken cancellationToken)
        {
            return Task.FromResult(_buffer
                    .Values
                    .Where(e => e.Timestamp >= date)
                    .AsEnumerable());
        }
    }

    public class MongoDbExpensesRepository : DataStore<Expense>, IExpensesRepository
    {
        public MongoDbExpensesRepository(IOptions<DbConfigurationSettings> options) : base(options, collection: "Expenses")
        {
        }

        public Task AddAsync(Expense expense, CancellationToken cancellationToken)
        {
            return base.InsertAsync(expense, cancellationToken);
        }

        public Task DelAsync(string id, CancellationToken cancellationToken)
        {
            return base.DeleteAsync(e => e.Id == id, cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetAsync(CancellationToken cancellationToken)
        {
            return base.FindAllAsync(cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetByCategoryAsync(string category, CancellationToken cancellationToken)
        {
            return base.FindAllAsync(expense => expense.Category == category, cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetByMonthAsync(DateTime date, CancellationToken cancellationToken)
        {
            return base.FindAllAsync(expense => expense.Timestamp >= date && expense.Timestamp < date.AddMonths(1), cancellationToken);
        }
    }

    public interface ITagsRepository
    {
        Task<IEnumerable<Tag>> GetAsync(CancellationToken cancellationToken);
        Task DelAsync(string tag, CancellationToken cancellationToken);
        Task AddAsync(TagDocument tag, CancellationToken cancellationToken);
        Task AppendTagsAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken);
        Task RemoveTagsAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken);
    }

    public class InMemoryTagsRepository : ITagsRepository
    {
        private int _counter;
        private readonly IList<Tag> _buffer;
        private readonly ILogger<ITagsRepository> _logger;

        public InMemoryTagsRepository(ILogger<ITagsRepository> logger)
        {
            _logger = logger;
            _counter = 0;
            _buffer = new List<Tag>();
        }

        public Task AddAsync(TagDocument tagDocument, CancellationToken cancellationToken)
        {
            tagDocument.Names.ForEach(n => _buffer.Add(new Tag { Name = n }));

            return Task.CompletedTask;
        }

        public Task DelAsync(string tagId, CancellationToken cancellationToken)
        {
            _buffer.Clear();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Tag>> GetAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult((IEnumerable<Tag>)_buffer);
        }

        public Task AppendTagsAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTagsAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }

    public class MongoDbTagsRepository : DataStore<TagDocument>, ITagsRepository
    {
        public MongoDbTagsRepository(IOptions<DbConfigurationSettings> options) : base(options, collection: "Tags")
        {
        }

        public Task AddAsync(TagDocument tag, CancellationToken cancellationToken)
        {
            return base.InsertAsync(tag, cancellationToken);
        }

        public async Task AppendTagsAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken)
        {
            var document = await base.FindOneAsync(_ => true, cancellationToken);
            var tagNames = tags.Select(t => t.Name).ToList();
            await base.AppendToArrayAsync(t => t.Id == document.Id, t => t.Names, tagNames, cancellationToken);
        }

        public Task DelAsync(string tagId, CancellationToken cancellationToken)
        {
            return base.DeleteAsync(t => t.Id == tagId, cancellationToken);
        }

        public async Task<IEnumerable<Tag>> GetAsync(CancellationToken cancellationToken)
        {
            var tagDocument = await base.FindOneAsync(_ => true, cancellationToken);
            return tagDocument.Names == null ? new List<Tag>() : tagDocument.Names.Select(n => new Tag { Name = n });
        }

        public async Task RemoveTagsAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken)
        {
            var document = await base.FindOneAsync(_ => true, cancellationToken);
            var tagNames = tags.Select(t => t.Name).ToList();
            await base.RemoveFromArrayAsync(t => t.Id == document.Id, t => t.Names, tagNames, cancellationToken);
        }
    }
}