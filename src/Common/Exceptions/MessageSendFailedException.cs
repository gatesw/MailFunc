using System;

namespace MailFunc.Common.Exceptions
{
    public class MessageSendFailedException : Exception
    {
        public MessageSendFailedException() : base("Failed to Send Message")
        {
        }
    }
}
