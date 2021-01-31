using Microsoft.Azure.Cosmos.Table;

namespace MailFunc.AzureTableStorage
{
    public class AzureTableStorageSenderConfigurationRepositoryOptions
    {
        public string TableName { get; set; } = null!;

        public string PartitionKey { get; set; } = null!;

        public bool CreateTableIfNotExists { get; set; }

        public TableClientConfiguration? TableClientOptions { get; set; }
    }
}
