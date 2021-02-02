using MailFunc.Common.Abstractions;
using MailFunc.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MailFunc.Common
{
    public abstract class SenderBase : ISender
    {
        private readonly ISenderConfigurationRepository _senderConfigurationRepository;
        private readonly IEnumerable<ITemplateApplicator> _templateApplicators;
        private readonly ISenderRequestValidator _senderRequestValidator;

        protected SenderBase(
            ISenderConfigurationRepository senderConfigurationRepository,
            IEnumerable<ITemplateApplicator> templateApplicators,
            ISenderRequestValidator senderRequestValidator)
        {
            _senderConfigurationRepository = senderConfigurationRepository;
            _templateApplicators = templateApplicators;
            _senderRequestValidator = senderRequestValidator;
        }

        public async Task Send(SenderRequest request, CancellationToken cancellationToken = default)
        {
            var configuration = await _senderConfigurationRepository.Retrieve(request.ConfigurationId) ??
                                throw new MissingConfigurationException(request.ConfigurationId);

            _senderRequestValidator.Validate(configuration, request);

            var from = request.FromEmail ?? configuration.DefaultFromEmail;
            var to = configuration.ToEmail;
            var messageBody = GetMessageBody(configuration, request);
            await Send(EmailMessage.Create(request, from, to, request.Subject, messageBody), cancellationToken);
        }

        protected abstract Task Send(EmailMessage message, CancellationToken cancellationToken = default);

        private string? GetMessageBody(ISenderConfiguration configuration, SenderRequest request)
        {
            if (configuration.TemplateType == null)
            {
                return request.Body;
            }

            var templateApplicator = _templateApplicators.FirstOrDefault(a => a.Supports(configuration));
            if (templateApplicator == null)
            {
                throw new NotSupportedException($"Template type {configuration.TemplateType} is not supported");
            }

            return templateApplicator.Apply(configuration, request);
        }
    }
}
