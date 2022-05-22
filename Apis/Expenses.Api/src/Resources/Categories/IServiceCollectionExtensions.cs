using Microsoft.Extensions.DependencyInjection;

namespace MyAssistant.Apis.Expenses.Api.Resources.Categories
{ 
    public static class IServiceCollectionExtensions
    { 
        public static IServiceCollection RegisterCategoriesFeatures(this IServiceCollection services) 
        {
            return services
                .AddSingleton<ICategoriesService, CategoriesService>()
                .AddSingleton<ICategoriesRepository, InMemoryRepository>();
        }
    }
}