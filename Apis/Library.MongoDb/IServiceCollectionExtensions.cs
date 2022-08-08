using Library.MongoDb.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.MongoDb
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<DbConfigurationSettings>(options => configuration.GetSection(DbConfigurationSettings.Section).Bind(options));
        }
    }
}
