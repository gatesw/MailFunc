using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MailFunc.Common;
using MailFunc.Common.Abstractions;
using MailFunc.Common.Exceptions;
using System.Web.Http;

namespace MailFunc.AzureFunction
{
    public class SendFunction
    {
        private readonly ISender _service;

        public SendFunction(ISender service)
        {
            _service = service;
        }

        [FunctionName("SendFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<SenderRequest>(requestBody);

                await _service.Send(request);

                return new OkResult();
            }
            catch (InvalidSenderRequestException e)
            {
                return new BadRequestObjectResult(e.ErrorMessages);
            }
            catch (MessageSendFailedException)
            {
                return new InternalServerErrorResult();
            }
            catch (MissingConfigurationException)
            {
                return new BadRequestResult();
            }
            catch (Exception e)
            {
                log.LogError("Unexpected Error Occured", e);
                return new InternalServerErrorResult();
            }
        }
    }
}
