using Microsoft.Extensions.DependencyInjection;
using MyAssistant.Apis.Expenses.Api.Resources.Expenses;

namespace MyAssistant.Apis.Expenses.Api.Resources.Historigrams
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterHistorigramsFeatures(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHistorigramService, HistorigramService>()
                .AddSingleton<IExpensesRepository, MongoDbExpensesRepository>();
        }
    }
}