using Library.MongoDb;
using Microsoft.Extensions.Logging;
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

    public class MongoDbExpensesRepository : IExpensesRepository
    {
        private readonly IDataStore<Expense, int> dataStore;

        public MongoDbExpensesRepository(IDataStore<Expense, int> dataStore)
        {
            this.dataStore = dataStore;
        }

        public Task AddAsync(Expense expense, CancellationToken cancellationToken)
        {
            return this.dataStore.InsertAsync(expense, cancellationToken);
        }

        public Task DelAsync(int id, CancellationToken cancellationToken)
        {
            return this.dataStore.DeleteAsync(e => e.Id == id, cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetAsync(CancellationToken cancellationToken)
        {
            return this.dataStore.FindAllAsync(cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetByCategoryAsync(string category, CancellationToken cancellationToken)
        {
            return this.dataStore.FindAllAsync(expense => expense.Category == category, cancellationToken);
        }

        public Task<IEnumerable<Expense>> GetFromDateAsync(DateTime date, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public interface ITagsRepository
    {
        Task<IEnumerable<Tag>> GetAsync(CancellationToken cancellationToken);
        Task DelAsync(int tag, CancellationToken cancellationToken);
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

        public Task DelAsync(int tagId, CancellationToken cancellationToken)
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

    public class MongoDbTagsRepository : ITagsRepository
    {
        private readonly IDataStore<TagDocument, int> dataStore;

        public MongoDbTagsRepository(IDataStore<TagDocument, int> dataStore)
        {
            this.dataStore = dataStore;
        }

        public Task AddAsync(TagDocument tag, CancellationToken cancellationToken)
        {
            return this.dataStore.InsertAsync(tag, cancellationToken);
        }

        public Task AppendTagsAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken)
        {
            var tagNames = tags.Select(t => t.Name).ToList();
            return this.dataStore.AppendToArrayAsync(t => t.Id == 0, t => t.Names, tagNames, cancellationToken);
        }

        public Task DelAsync(int tagId, CancellationToken cancellationToken)
        {
            return this.dataStore.DeleteAsync(t => t.Id == tagId, cancellationToken);
        }

        public async Task<IEnumerable<Tag>> GetAsync(CancellationToken cancellationToken)
        {
            var tagDocument = await this.dataStore.FindOneAsync(t => t.Id == 0, cancellationToken);
            return tagDocument.Names == null ? new List<Tag>() : tagDocument.Names.Select(n => new Tag { Name = n });
        }

        public Task RemoveTagsAsync(IEnumerable<Tag> tags, CancellationToken cancellationToken)
        {
            var tagNames = tags.Select(t => t.Name).ToList();
            return this.dataStore.RemoveFromArrayAsync(t => t.Id == 0, t => t.Names, tagNames, cancellationToken);
        }
    }
}