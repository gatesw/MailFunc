using MailFunc.Common.Exceptions;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
using MailFunc.Common.Abstractions;
using MailFunc.Common;
using System.Collections.Generic;
using System.Threading;

namespace MailFunc.SendGrid
{
    public class Sender : SenderBase, ISender
    {
        private readonly ILogger<Sender> _logger;
        private readonly ISendGridClient _client;

        public Sender(
            ILogger<Sender> logger,
            ISendGridClient client,
            ISenderConfigurationRepository senderConfigurationRepository,
            IEnumerable<ITemplateApplicator> templateApplicators,
            ISenderRequestValidator senderRequestValidator) 
            : base(senderConfigurationRepository, templateApplicators, senderRequestValidator)
        {
            _logger = logger;
            _client = client;
        }

        protected override async Task Send(EmailMessage message, CancellationToken cancellationToken = default)
        {
            var msg = message.OriginalRequest.BodyIsHtml ?
                      MailHelper.CreateSingleEmail(new EmailAddress(message.From), new EmailAddress(message.To), message.Subject, null, message.Body) :
                      MailHelper.CreateSingleEmail(new EmailAddress(message.From), new EmailAddress(message.To), message.Subject, message.Body, null);

            var response = await _client.SendEmailAsync(msg, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.DeserializeResponseBodyAsync(response.Body);
                _logger.LogError($"Failed to Send Message for Configuration {message.OriginalRequest.ConfigurationId}", body);
                throw new MessageSendFailedException();
            }
        }
    }
}
