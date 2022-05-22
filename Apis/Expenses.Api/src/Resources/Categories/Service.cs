using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyAssistant.Apis.Expenses.Api.Resources.Categories 
{
    public interface ICategoriesService 
    {
        Task<IEnumerable<Category>> GetAsync(CancellationToken cancellationToken);
        Task AddAsync(Category category, CancellationToken cancellationToken);
        Task DelAsync(string categoryName, CancellationToken cancellationToken);
    }

    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _repository;
        private readonly ILogger<CategoriesService> _logger;

        public CategoriesService(ICategoriesRepository repository, ILogger<CategoriesService> logger) 
        {
            _logger = logger;
            _repository = repository;
        }

        public Task AddAsync(Category category, CancellationToken cancellationToken)
        {
            return _repository.AddAsync(category, cancellationToken);
        }

        public async Task DelAsync(string categoryName, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByNameAsync(categoryName, cancellationToken);

            if (category is not null)
                await _repository.DelAsync(category.Id, cancellationToken);
        }

        public Task<IEnumerable<Category>> GetAsync(CancellationToken cancellationToken)
        {
            return _repository.GetAsync(cancellationToken);
        }
    }
}


