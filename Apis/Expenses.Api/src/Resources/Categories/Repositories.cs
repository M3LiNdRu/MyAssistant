using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.MongoDb;
using Library.MongoDb.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MyAssistant.Apis.Expenses.Api.Resources.Categories
{
    public interface ICategoriesRepository
    {
        Task AddAsync(Category category, CancellationToken cancellationToken);
        Task DelAsync(int categoryId, CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetAsync(CancellationToken cancellationToken);
        Task<Category> GetByNameAsync(string categoryName, CancellationToken cancellationToken);
    }

    public class InMemoryRepository : ICategoriesRepository
    {
        private int _counter;
        private readonly IDictionary<int, Category> _buffer;
        private readonly ILogger<InMemoryRepository> _logger;

        public InMemoryRepository(ILogger<InMemoryRepository> logger) 
        {
            _logger = logger;
            _buffer = new Dictionary<int, Category>();
            _counter = 0;
        }
        public Task AddAsync(Category category, CancellationToken cancellationToken)
        {
            category.Id = _counter;

            _buffer.Add(category.Id, category);

            _counter++;
            
            return Task.CompletedTask;
        }

        public Task DelAsync(int categoryId, CancellationToken cancellationToken)
        {
            if (!_buffer.Remove(categoryId)) 
            {
                return Task.FromException(new KeyNotFoundException($"CategoryId {categoryId} was not found"));
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Category>> GetAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_buffer.Values.AsEnumerable());
        }

        public Task<Category> GetByNameAsync(string categoryName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_buffer
                            .Values
                            .SingleOrDefault(c => c.Name
                                                    .Equals(categoryName, System.StringComparison.OrdinalIgnoreCase)));
        }
    }

    public class MongoDbRepository : DataStore<Category, int>, ICategoriesRepository
    {
        public MongoDbRepository(IOptions<DbConfigurationSettings> options) : base(options, "Categories")
        {
        }

        public Task AddAsync(Category category, CancellationToken cancellationToken)
        {
            return base.InsertAsync(category, cancellationToken);
        }

        public Task DelAsync(int categoryId, CancellationToken cancellationToken)
        {
            return base.DeleteAsync(c => c.Id == categoryId, cancellationToken);
        }

        public Task<IEnumerable<Category>> GetAsync(CancellationToken cancellationToken)
        {
            return base.FindAllAsync(cancellationToken);
        }

        public Task<Category> GetByNameAsync(string categoryName, CancellationToken cancellationToken)
        {
            return base.FindOneAsync(c => c.Name == categoryName, cancellationToken);
        }
    }
}