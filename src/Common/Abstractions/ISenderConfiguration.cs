using System;

namespace MailFunc.Common.Abstractions
{
    public interface ISenderConfiguration
    {
        Guid Id { get; }

        string Name { get; }

        string ToEmail { get;  }

        string DefaultFromEmail { get; }

        bool AllowEmptySubject { get; }

        bool AllowEmptyBody { get; }
    }
}
