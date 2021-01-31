using Microsoft.Azure.Cosmos.Table;

namespace MailFunc.AzureTableStorage
{
    public interface IStorageAccountRetriever
    {
        CloudStorageAccount Retrieve();
    }
}
