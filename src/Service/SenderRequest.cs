using System;

namespace MailFunc.Common
{
    public class SenderRequest
    {
        public Guid ConfigurationId { get; set; }

        public string? FromEmail { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public bool BodyIsHtml { get; set; }
    }
}
