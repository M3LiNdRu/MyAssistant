using Microsoft.Extensions.DependencyInjection;

namespace MyAssistant.Apis.Expenses.Api.Resources.Appointments
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAppointmentsFeatures(this IServiceCollection services)
        {
            return services
                .AddSingleton<IAppointmentsService, AppointmentsService>()
                .AddSingleton<IAppointmentsRepository, MongoDbAppointmentsRepository>();
        }
    }
}
