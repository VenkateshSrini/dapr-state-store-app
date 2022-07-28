using Dapr.Client;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dapr.state.console.extensions.service
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDaprClient(this IServiceCollection services)
        {
            services.TryAddSingleton(_ =>
            {
                var builder = new DaprClientBuilder();
                 return builder.Build();
            });
            return services;
        }
    }
}
