using Library.MongoDb;
using Library.MongoDb.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyAssistant.Apis.Expenses.Api.Resources.Tags
{
    public interface ITagsRepository
    {
        Task<IEnumerable<Tag>> GetAsync(CancellationToken cancellationToken);
    }

    public class MongoDbTagsRepository : DataStore<TagDocument>, ITagsRepository
    {
        public MongoDbTagsRepository(IOptions<DbConfigurationSettings> options) : base(options, collection: "Tags")
        {
        }

        public async Task<IEnumerable<Tag>> GetAsync(CancellationToken cancellationToken)
        {
            var tagDocument = await base.FindOneAsync(_ => true, cancellationToken);
            return tagDocument.Names == null ? new List<Tag>() : tagDocument.Names.Select(n => new Tag { Name = n });
        }

    }
}