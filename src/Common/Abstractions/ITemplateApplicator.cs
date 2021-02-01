namespace MailFunc.Common.Abstractions
{
    public interface ITemplateApplicator
    {
        string Apply(ISenderConfiguration configuration, SenderRequest request);

        bool Supports(ISenderConfiguration configuration);
    }
}
