using MailFunc.Common.Exceptions;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
using MailFunc.Common.Abstractions;
using MailFunc.Common;
using System.Threading;

namespace MailFunc.SendGrid
{
    public class Sender : ISender
    {
        private readonly ILogger<Sender> _logger;
        private readonly ISendGridClient _client;
        private readonly ISenderConfigurationRepository _senderConfigurationRepository;
        private readonly ISenderRequestValidator _senderRequestValidator;

        public Sender(
            ILogger<Sender> logger,
            ISendGridClient client,
            ISenderConfigurationRepository senderConfigurationRepository,
            ISenderRequestValidator senderRequestValidator)
        {
            _logger = logger;
            _client = client;
            _senderConfigurationRepository = senderConfigurationRepository;
            _senderRequestValidator = senderRequestValidator;
        }

        public async Task Send(SenderRequest request, CancellationToken cancellationToken = default)
        {
            var configuration = await _senderConfigurationRepository.Retrieve(request.ConfigurationId) ??
                                throw new MissingConfigurationException(request.ConfigurationId);

            _senderRequestValidator.Validate(configuration, request);

            var from = new EmailAddress(request.FromEmail ?? configuration.DefaultFromEmail);
            var to = new EmailAddress(configuration.ToEmail, configuration.Name);
            var msg = request.BodyIsHtml ?
                      MailHelper.CreateSingleEmail(from, to, request.Subject, null, request.Body) :
                      MailHelper.CreateSingleEmail(from, to, request.Subject, request.Body, null);

            var response = await _client.SendEmailAsync(msg, cancellationToken);
            if(!response.IsSuccessStatusCode)
            {
                var body = await response.DeserializeResponseBodyAsync(response.Body);
                _logger.LogError($"Failed to Send Message for Configuration {request.ConfigurationId}", body);
                throw new MessageSendFailedException();
            }
        }
    }
}
