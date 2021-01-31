using MailFunc.Common;
using Microsoft.Azure.Cosmos.Table;
using System;

namespace MailFunc.AzureTableStorage
{
    public class SenderConfigurationTableEntity : TableEntity
    {
        public SenderConfigurationTableEntity()
        {
        }

        private SenderConfigurationTableEntity(string partitionKey, SenderConfiguration source)
               : base(partitionKey, source.Id.ToString())
        {
            Id = source.Id;
            Name = source.Name;
            ToEmail = source.ToEmail;
            DefaultFromEmail = source.DefaultFromEmail;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string ToEmail { get; set; } = null!;

        public string DefaultFromEmail { get; set; } = null!;

        public static SenderConfigurationTableEntity Create(string partitionKey, SenderConfiguration source) =>
            new SenderConfigurationTableEntity(partitionKey, source);
    }
}
