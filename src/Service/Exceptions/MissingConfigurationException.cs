using System;

namespace MailFunc.Common.Exceptions
{
    public class MissingConfigurationException : NotSupportedException
    {
        public MissingConfigurationException(Guid configurationId) 
            : base($"No configuration exists for '{configurationId}'")
        {
        }
    }
}
