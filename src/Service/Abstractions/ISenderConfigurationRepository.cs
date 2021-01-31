using MailFunc;
using System;
using System.Threading.Tasks;

namespace MailFunc.Common.Abstractions
{
    public interface ISenderConfigurationRepository
    {
        public Task<SenderConfiguration?> Retrieve(Guid id);
    }
}
