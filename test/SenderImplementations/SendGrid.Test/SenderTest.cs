using MailFunc.Common;
using MailFunc.Common.Abstractions;
using MailFunc.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MailFunc.SendGrid.Test
{
    public class SenderTest
    {
        private readonly Sender _sender;
        private readonly Mock<ISendGridClient> _mockSendGridClient;
        private readonly Mock<ISenderConfigurationRepository> _mockSenderConfigurationRepository;
        private readonly Mock<ISenderRequestValidator> _mockSenderRequestValidator;

        public SenderTest()
        {
            _mockSendGridClient = new Mock<ISendGridClient>();
            _mockSenderConfigurationRepository = new Mock<ISenderConfigurationRepository>();
            _mockSenderRequestValidator = new Mock<ISenderRequestValidator>();

            _sender = new Sender(
                Mock.Of<ILogger<Sender>>(),
                _mockSendGridClient.Object,
                _mockSenderConfigurationRepository.Object,
                _mockSenderRequestValidator.Object);
        }

        [Fact]
        public Task Send_WithInvalidConfigurationId_ShouldThrowMissingConfigurationException()
        {
            _mockSenderConfigurationRepository.Setup(s => s.Retrieve(It.IsAny<Guid>())).ReturnsAsync((SenderConfiguration?)null);

            return Assert.ThrowsAsync<MissingConfigurationException>(() => _sender.Send(new SenderRequest()));
        }

        [Fact]
        public Task Send_SendGridClientReturnsNonSuccessResponse_ShouldThrowMessageSendFailedException()
        {
            var senderConfiguration = new SenderConfiguration
            {
                Id = Guid.NewGuid(),
                ToEmail = "test@email.com",
                DefaultFromEmail = "test@email.com",
                Name = "Test",
            };

            var message = new HttpResponseMessage();
            var headers = message.Headers;
            var content = new StringContent("{ errors: [] }");
            var response = new Response(System.Net.HttpStatusCode.Unauthorized, content, headers);

            _mockSenderConfigurationRepository.Setup(s => s.Retrieve(It.IsAny<Guid>())).ReturnsAsync(senderConfiguration);
            _mockSendGridClient.Setup(s => s.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            return Assert.ThrowsAsync<MessageSendFailedException>(() => _sender.Send(new SenderRequest()));
        }

        [Fact]
        public async Task Send_ShouldSendEmailAsync_OnSendGridClient()
        {
            var senderConfiguration = new SenderConfiguration
            {
                Id = Guid.NewGuid(),
                ToEmail = "test@email.com",
                DefaultFromEmail = "test@email.com",
                Name = "Test",
            };

            var message = new HttpResponseMessage();
            var headers = message.Headers;
            var content = new StringContent(string.Empty);
            var response = new Response(System.Net.HttpStatusCode.OK, content, headers);

            _mockSenderConfigurationRepository.Setup(s => s.Retrieve(It.IsAny<Guid>())).ReturnsAsync(senderConfiguration);
            _mockSendGridClient.Setup(s => s.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            await _sender.Send(new SenderRequest());

            _mockSendGridClient.Verify(s => s.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
