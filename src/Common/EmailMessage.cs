using System;
using System.Collections.Generic;
using System.Text;

namespace MailFunc.Common
{
    public class EmailMessage
    {
        private EmailMessage(SenderRequest originalRequest, string from, string to, string? subject, string? body)
        {
            OriginalRequest = originalRequest;
            From = from;
            To = to;
            Subject = Subject;
            Body = body;
        }

        public SenderRequest OriginalRequest { get; }

        public string From { get; }

        public string To { get; }

        public string? Subject { get; }

        public string? Body { get; }

        public static EmailMessage Create(SenderRequest originalRequest, string from, string to, string? subject = null, string? body = null) =>
            new EmailMessage(originalRequest, from, to, subject, body);
    }
}
