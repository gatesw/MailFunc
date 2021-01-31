using Microsoft.Azure.Cosmos.Table;

namespace MailFunc.AzureTableStorageRepository
{
    public interface IStorageAccountRetriever
    {
        CloudStorageAccount Retrieve();
    }
}
