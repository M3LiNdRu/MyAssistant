using Library.MongoDb;
using Microsoft.Extensions.DependencyInjection;
using MyAssistant.Apis.Expenses.Api.Resources.Expenses;

namespace MyAssistant.Apis.Expenses.Api.Resources.Summary
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterExpensesFeatures(this IServiceCollection services)
        {
            return services
                .AddSingleton<ISummaryService, SummaryService>()
                .AddSingleton<IExpensesRepository, MongoDbExpensesRepository>()
                .AddSingleton<IDataStore<Expense, int>, DataStore<Expense, int>>();
        }
    }
}