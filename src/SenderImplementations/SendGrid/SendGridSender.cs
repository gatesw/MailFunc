using MailFunc.Common.Exceptions;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
using MailFunc.Common.Abstractions;
using MailFunc.Common;

namespace MailFunc.SendGrid
{
    public class Sender : ISender
    {
        private readonly ILogger<Sender> _logger;
        private readonly ISendGridClient _client;
        private readonly ISenderConfigurationRepository _senderConfigurationRepository;

        public Sender(
            ILogger<Sender> logger,
            ISendGridClient client,
            ISenderConfigurationRepository senderConfigurationRepository)
        {
            _logger = logger;
            _client = client;
            _senderConfigurationRepository = senderConfigurationRepository;
        }

        public async Task Send(SenderRequest request)
        {
            var configuration = await _senderConfigurationRepository.Retrieve(request.ConfigurationId) ??
                                throw new MissingConfigurationException(request.ConfigurationId);

            var from = new EmailAddress(request.FromEmail ?? configuration.DefaultFromEmail);
            var to = new EmailAddress(configuration.ToEmail, configuration.Name);
            var msg = request.BodyIsHtml ?
                      MailHelper.CreateSingleEmail(from, to, request.Subject, null, request.Body) :
                      MailHelper.CreateSingleEmail(from, to, request.Subject, request.Body, null);

            var response = await _client.SendEmailAsync(msg);
            if(!response.IsSuccessStatusCode)
            {
                var body = await response.DeserializeResponseBodyAsync(response.Body);
                _logger.LogError($"Failed to Send Message for Configuration {request.ConfigurationId}", body);
                throw new MessageSendFailedException();
            }
        }
    }
}
