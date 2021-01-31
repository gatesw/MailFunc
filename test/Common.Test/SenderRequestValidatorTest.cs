using MailFunc.Common;
using MailFunc.Common.Exceptions;
using Xunit;

namespace MailFunc.SendGrid.Test
{
    public class SenderRequestValidatorTest
    {
        private readonly SenderRequestValidator _validator;

        public SenderRequestValidatorTest()
        {
            _validator = new SenderRequestValidator();
        }

        [Fact]
        public void Validate_WithBrokenRule_ShouldThrowInvalidSenderRequestException()
        {
            var configuration = new SenderConfiguration
            {
                AllowEmptySubject = false,
            };

            var request = new SenderRequest
            {
                Subject = null,
            };

            Assert.Throws<InvalidSenderRequestException>(() => _validator.Validate(configuration, request));
        }

        [Theory]
        [InlineData(true, null)]
        [InlineData(true, "")]
        [InlineData(true, " ")]
        [InlineData(false, "Test")]
        public void HasSubject_ShouldNotReturnErrorMessage(bool allowEmptySubject, string? subject)
        {
            var configuration = new SenderConfiguration
            {
                AllowEmptySubject = allowEmptySubject,
            };

            var request = new SenderRequest
            {
                Subject = subject,
            };

            var messages = _validator.HasSubject(configuration, request);

            Assert.Empty(messages);
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(false, "")]
        [InlineData(false, " ")]
        public void HasSubject_ShouldReturnErrorMessage(bool allowEmptySubject, string? subject)
        {
            var configuration = new SenderConfiguration
            {
                AllowEmptySubject = allowEmptySubject,
            };

            var request = new SenderRequest
            {
                Subject = subject,
            };

            var messages = _validator.HasSubject(configuration, request);

            Assert.NotEmpty(messages);
        }

        [Theory]
        [InlineData(true, null)]
        [InlineData(true, "")]
        [InlineData(true, " ")]
        [InlineData(false, "Test")]
        public void HasBody_ShouldNotReturnErrorMessage(bool allowEmptyBody, string? body)
        {
            var configuration = new SenderConfiguration
            {
                AllowEmptyBody = allowEmptyBody,
            };

            var request = new SenderRequest
            {
                Body = body,
            };

            var messages = _validator.HasBody(configuration, request);

            Assert.Empty(messages);
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(false, "")]
        [InlineData(false, " ")]
        public void HasBody_ShouldReturnErrorMessage(bool allowEmptyBody, string? body)
        {
            var configuration = new SenderConfiguration
            {
                AllowEmptyBody = allowEmptyBody,
            };

            var request = new SenderRequest
            {
                Body = body,
            };

            var messages = _validator.HasBody(configuration, request);

            Assert.NotEmpty(messages);
        }
    }
}
