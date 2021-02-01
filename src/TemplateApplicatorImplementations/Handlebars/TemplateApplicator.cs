using MailFunc.Common;
using MailFunc.Common.Abstractions;
using MailFunc.Common.Exceptions;
using Newtonsoft.Json;
using System;

namespace MailFunc.Handlebars
{
    public class TemplateApplicator : ITemplateApplicator
    {
        public const string TemplateType = "Handlebars";

        public string Apply(ISenderConfiguration configuration, SenderRequest request)
        {
            if (string.IsNullOrWhiteSpace(configuration.Template))
            {
                throw new MissingTemplateExcpetion(configuration.Id);
            }

            if (string.IsNullOrWhiteSpace(request.Body))
            {
                return string.Empty;
            }

            var template = HandlebarsDotNet.Handlebars.Compile(configuration.Template);
            var data = JsonConvert.DeserializeObject(request.Body);
            return template(data);
        }

        public bool Supports(ISenderConfiguration configuration) => TemplateType.Equals(configuration.TemplateType, StringComparison.InvariantCultureIgnoreCase);
    }
}
