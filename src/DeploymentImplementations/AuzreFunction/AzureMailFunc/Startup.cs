using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using MailFunc.AzureTableStorageRepository;
using System;
using MainFunc.SendGridSender;

[assembly: FunctionsStartup(typeof(MailFunc.AzureFunction.Startup))]
namespace MailFunc.AzureFunction
{
    public class Startup : FunctionsStartup
    {
        private const string AzureTableStorageConnectionStringKey = "AzureTableStorageConnectionString";
        private const string AzureTableStorageMailFuncTableNameKey = "AzureTableStorageMailFuncTableName";
        private const string AzureTableStorageMailFuncTablePartitionKey = "MailFunc";
        private const string SendGridApiKey = "SendGridApiKey";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable(AzureTableStorageConnectionStringKey)
                                   ?? throw new NotSupportedException("Azure Table Storage Connection String required to use Mail Func");

            builder.Services.UseAzureTableStorage(
                connectionString,
                options => {
                    options.CreateTableIfNotExists = true;
                    options.TableName = Environment.GetEnvironmentVariable(AzureTableStorageMailFuncTableNameKey)
                                        ?? throw new NotSupportedException("Table Name required to use Mail Func");
                    options.PartitionKey = AzureTableStorageMailFuncTablePartitionKey;
                    options.TableClientOptions = null;
                });

            builder.Services.UseSendGrid(options => {
                options.ApiKey = Environment.GetEnvironmentVariable(SendGridApiKey);
            });
        }
    }
}
