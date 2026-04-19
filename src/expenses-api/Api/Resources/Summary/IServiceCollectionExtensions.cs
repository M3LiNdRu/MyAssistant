using Microsoft.Extensions.DependencyInjection;
using MyAssistant.Apis.Expenses.Api.Resources.Expenses;

namespace MyAssistant.Apis.Expenses.Api.Resources.Summary
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSummaryFeatures(this IServiceCollection services)
        {
            return services
                .AddSingleton<ISummaryService, SummaryService>()
                .AddSingleton<IExpensesRepository, MongoDbExpensesRepository>();
        }
    }
}