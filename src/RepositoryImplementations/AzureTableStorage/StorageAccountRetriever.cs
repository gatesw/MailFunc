using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;

namespace MailFunc.AzureTableStorage
{
    public class StorageAccountRetriever : IStorageAccountRetriever
    {
        private ILogger<StorageAccountRetriever> _logger;
        private readonly string _connectionString;
        private CloudStorageAccount? _cloudStorageAccount;

        public StorageAccountRetriever(ILogger<StorageAccountRetriever> logger, string connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        public CloudStorageAccount Retrieve() => _cloudStorageAccount ?? SetStorageAccount();

        private CloudStorageAccount SetStorageAccount()
        {
            try
            {
                _cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
                return _cloudStorageAccount;
            }
            catch (FormatException)
            {
                _logger.LogError("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid");
                throw;
            }
            catch (ArgumentException)
            {
                _logger.LogError("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid");
                throw;
            }
        }
    }
}
