using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SendGrid;
using MailFunc.Common.Abstractions;
using System;
using MailFunc.SendGrid;

namespace MainFunc.SendGrid
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseSendGrid(
            this IServiceCollection services,
            Action<SendGridClientOptions> options)
        {
            services.Configure(options);
            services.AddScoped<ISendGridClient>(sp => new SendGridClient(sp.GetRequiredService<IOptions<SendGridClientOptions>>().Value));
            services.AddScoped<ISender, Sender>();
            return services;
        }
    }
}
