using Core.Entities;
using Core.Abstract;
using Infrastructure.CosmosDbData.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    /// <summary>
    ///     Extension methods for IApplicationBuilder 
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Ensure Cosmos DB is created
        /// </summary>
        /// <param name="builder"></param>
        public static void EnsureCosmosDbIsCreated(this IApplicationBuilder builder)
        {
            using (IServiceScope serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ICosmosDbContainerFactory factory = serviceScope.ServiceProvider.GetService<ICosmosDbContainerFactory>();

                factory.EnsureDbSetupAsync().Wait();
            }
        }
    }
}
