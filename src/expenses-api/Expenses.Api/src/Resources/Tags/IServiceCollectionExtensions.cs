using Microsoft.Extensions.DependencyInjection;

namespace MyAssistant.Apis.Expenses.Api.Resources.Tags
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterTagsFeatures(this IServiceCollection services)
        {
            return services
                .AddSingleton<ITagsRepository, MongoDbTagsRepository>()
                .AddSingleton<ITagsService, TagsService>();
        }
    }
}