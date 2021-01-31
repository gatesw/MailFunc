using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SendGrid;
using MailFunc.Common.Abstractions;
using System;
using MailFunc.SendGrid;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MainFunc.SendGrid
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseSendGrid(
            this IServiceCollection services,
            Action<SendGridClientOptions> options)
        {
            services.Configure(options);
            services.TryAddScoped<ISendGridClient>(sp => new SendGridClient(sp.GetRequiredService<IOptions<SendGridClientOptions>>().Value));
            services.TryAddScoped<ISenderRequestValidator, ISenderRequestValidator>();
            services.TryAddScoped<ISender, Sender>();
            return services;
        }
    }
}
