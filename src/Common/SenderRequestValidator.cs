using MailFunc.Common.Abstractions;
using MailFunc.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace MailFunc.Common
{
    public class SenderRequestValidator : ISenderRequestValidator
    {
        public void Validate(ISenderConfiguration configuration, SenderRequest request)
        {
            var results = new[]
            {
                HasSubject(configuration, request),
                HasBody(configuration, request),
            };

            var errorMessages = results.SelectMany(m => m);
            if (errorMessages.Any())
            {
                throw new InvalidSenderRequestException(errorMessages);
            }
        }

        public IEnumerable<string> HasSubject(ISenderConfiguration configuration, SenderRequest request)
        {
            if(!configuration.AllowEmptySubject && string.IsNullOrWhiteSpace(request.Subject))
            {
                yield return "Message subject cannot be empty";
            }
        }

        public IEnumerable<string> HasBody(ISenderConfiguration configuration, SenderRequest request)
        {
            if (!configuration.AllowEmptyBody && string.IsNullOrWhiteSpace(request.Body))
            {
                yield return "Message body cannot be empty";
            }
        }
    }
}
