using System;

namespace MailFunc.Common
{
    public class SenderConfiguration
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string ToEmail { get; set; } = null!;

        public string DefaultFromEmail { get; set; } = null!;
    }
}
