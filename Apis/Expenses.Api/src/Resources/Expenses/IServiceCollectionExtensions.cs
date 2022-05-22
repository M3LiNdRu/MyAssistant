using Microsoft.Extensions.DependencyInjection;

namespace MyAssistant.Apis.Expenses.Api.Resources.Expenses
{ 
    public static class IServiceCollectionExtensions
    { 
        public static IServiceCollection RegisterExpensesFeatures(this IServiceCollection services) 
        {
            return services
                .AddSingleton<IExpensesService, ExpensesService>()
                .AddSingleton<IExpensesRepository, InMemoryExpensesRepository>();
        }
    }
}