using System;

namespace MailFunc.Common.Exceptions
{
    public class MissingTemplateExcpetion : NotSupportedException
    {
        public MissingTemplateExcpetion(Guid configurationId)
            : base($"Missing template for configuration {configurationId}")
        {
        }
    }
}
