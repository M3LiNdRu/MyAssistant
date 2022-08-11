using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyAssistant.Apis.Expenses.Api.Resources.Tags
{
    public interface ITagsService
    {
        Task<Response> GetTagNamesAsync(CancellationToken cancellationToken);
    }

    public class TagsService : ITagsService
    {
        private readonly ITagsRepository _repository;

        public TagsService(ITagsRepository repository)
        {
            this._repository = repository;
        }

        public async Task<Response> GetTagNamesAsync(CancellationToken cancellationToken)
        {
            var response = new Response();

            var tags = await _repository.GetAsync(cancellationToken);

            response.Tags = tags.Select(t => t.Name).ToList();

            return response;
        }
    }
}
