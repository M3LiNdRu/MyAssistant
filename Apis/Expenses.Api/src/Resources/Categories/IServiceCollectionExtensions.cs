using Microsoft.Extensions.DependencyInjection;

namespace MyAssistant.Apis.Expenses.Api.Resources.Categories
{ 
    public static class IServiceCollectionExtensions
    { 
        public static void RegisterCategoriesFeatures(this IServiceCollection services) 
        {
            services
                .AddSingleton<ICategoriesService, CategoriesService>()
                .AddSingleton<ICategoriesRepository, InMemoryRepository>();
        }
    }
}