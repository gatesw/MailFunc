using System.Threading;
using System.Threading.Tasks;

namespace MailFunc.Common.Abstractions
{
    public interface ISender
    {
        Task Send(SenderRequest request, CancellationToken cancellationToken = default);
    }
}
