using Library.MongoDb;
using Microsoft.Extensions.DependencyInjection;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{ 
    public static class IServiceCollectionExtensions
    { 
        public static IServiceCollection RegisterExpensesFeatures(this IServiceCollection services) 
        {
            return services
                .AddSingleton<IExpensesService, ExpensesService>()
                .AddSingleton<IExpensesRepository, MongoDbExpensesRepository>()
                .AddSingleton<IDataStore<Expense, int>, DataStore<Expense, int>>()
                .AddSingleton<IDataStore<TagDocument, int>, DataStore<TagDocument, int>>()
                .AddSingleton<ITagsRepository, MongoDbTagsRepository>();
        }
    }
}