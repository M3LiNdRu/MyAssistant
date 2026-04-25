using Microsoft.Extensions.DependencyInjection;

namespace MyAssistant.Apis.Expenses.Api.Resources.Investments
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterInvestmentsFeatures(this IServiceCollection services)
        {
            return services
                .AddSingleton<IPortfoliosService, PortfoliosService>()
                .AddSingleton<IPortfoliosRepository, MongoDbPortfoliosRepository>()
                .AddSingleton<IInvestmentsService, InvestmentsService>()
                .AddSingleton<IInvestmentsRepository, MongoDbInvestmentsRepository>()
                .AddSingleton<ITransactionsService, TransactionsService>()
                .AddSingleton<ITransactionsRepository, MongoDbTransactionsRepository>();
        }
    }
}
