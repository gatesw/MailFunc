namespace MailFunc.Common.Abstractions
{
    public interface ISenderRequestValidator
    {
        void Validate(ISenderConfiguration configuration, SenderRequest request);
    }
}
