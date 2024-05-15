using Core.Entities;
using Core.Abstract;
using Infrastructure.CosmosDbData.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.CosmosDbData.Repository
{
    public class ApplicationFormRepository : CosmosDbRepository<ApplicationForm>, IApplicationFormRepository
    {
        /// <summary>
        ///     CosmosDB container name
        /// </summary>
        public override string ContainerName { get; } = "ApplicationForm";

        /// <summary>
        ///     Generate Id.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override string GenerateId(ApplicationForm entity) => $"{Guid.NewGuid()}";

        /// <summary>
        ///     Returns the value of the partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public ApplicationFormRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }

        // Use Cosmos DB Parameterized Query to avoid SQL Injection.
        public async Task<IEnumerable<ApplicationForm>> GetAllItemsAsync()
        {
            string query = @"SELECT * FROM c";

            QueryDefinition queryDefinition = new QueryDefinition(query);
            string queryString = queryDefinition.QueryText;

            IEnumerable<ApplicationForm> entities = await this.GetItemsAsync(queryString);

            return entities;
        }
    }
}
