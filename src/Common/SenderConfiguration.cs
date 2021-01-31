using MailFunc.Common.Abstractions;
using System;

namespace MailFunc.Common
{
    public class SenderConfiguration : ISenderConfiguration
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string ToEmail { get; set; } = null!;

        public string DefaultFromEmail { get; set; } = null!;

        public bool AllowEmptySubject { get; set; }

        public bool AllowEmptyBody { get; set; }
    }
}
