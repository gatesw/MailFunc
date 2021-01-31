using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MailFunc.Common.Exceptions
{
    public class InvalidSenderRequestException : ValidationException
    {
        public InvalidSenderRequestException(IEnumerable<string> errorMessages)
        {
            ErrorMessages = errorMessages.ToList();
        }

        public List<string> ErrorMessages { get; }
    }
}
