using MailFunc.Common.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MailFunc.Handlebars
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection UseHandlebars(this IServiceCollection services)
        {
            services.AddScoped<ITemplateApplicator, TemplateApplicator>();
            return services;
        }
    }
}
