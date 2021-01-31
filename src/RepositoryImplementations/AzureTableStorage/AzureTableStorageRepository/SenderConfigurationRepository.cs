using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailFunc.Common.Abstractions;
using System;
using System.Threading.Tasks;
using MailFunc.Common;

namespace MailFunc.AzureTableStorageRepository
{
    public class SenderConfigurationRepository : ISenderConfigurationRepository
    {
        private ILogger<SenderConfigurationRepository> _logger;
        private IOptions<AzureTableStorageSenderConfigurationRepositoryOptions> _options;
        private readonly IStorageAccountRetriever _storageAccountRetriever;

        public SenderConfigurationRepository(
            ILogger<SenderConfigurationRepository> logger,
            IOptions<AzureTableStorageSenderConfigurationRepositoryOptions> options,
            IStorageAccountRetriever storageAccountRetriever)
        {
            _logger = logger;
            _options = options;
            _storageAccountRetriever = storageAccountRetriever;
        }

        public async Task<SenderConfiguration?> Retrieve(Guid id)
        {
            var table = await GetTable();
            var tableOperation = TableOperation.Retrieve<SenderConfigurationTableEntity>(_options.Value.PartitionKey, id.ToString());
            var queryResult = await table.ExecuteAsync(tableOperation);
            if(queryResult.Result is SenderConfiguration configuration)
            {
                return configuration;
            }

            return null;
        }

        private CloudTableClient GetTableClient() => _storageAccountRetriever.Retrieve().CreateCloudTableClient(_options.Value.TableClientOptions);

        private async Task<CloudTable> GetTable()
        {
            var tableClient = GetTableClient();
            var table = tableClient.GetTableReference(_options.Value.TableName);
            if (table.Exists())
            {
                return table;
            }

            if (_options.Value.CreateTableIfNotExists)
            {
                await table.CreateAsync();
                return table;
            }

            _logger.LogError($"No Table exists for {_options.Value.TableName} and CreateTableIfNotExists set to false");
            throw new InvalidOperationException($"No Table exists");
        }
    }
}
