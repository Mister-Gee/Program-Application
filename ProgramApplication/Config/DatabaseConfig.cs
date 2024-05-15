using Infrastructure.Model;
using Infrastructure.Extensions;
using Core.Abstract;
using Infrastructure.CosmosDbData.Repository;

namespace ProgramApplication.Config
{
    public static class DatabaseConfig
    {
        /// <summary>
        ///     Setup Cosmos DB
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupCosmosDb(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind database-related bindings
            CosmosDbSettings cosmosDbConfig = configuration.GetSection("ConnectionStrings:CosmosDB").Get<CosmosDbSettings>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);

            services.AddScoped<IApplicationFormRepository, ApplicationFormRepository>();
        }

    }
}
