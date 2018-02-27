using Consul;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Heimdall.Gateway.Core;
using Heimdall.Gateway.Registry;
using Heimdall.Gateway.Registry.Clients;
using Heimdall.Gateway.ServiceDiscovery.Clients;

namespace Heimdall.Gateway.ServiceDiscovery
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Register the entities required for this component
        /// Expected to be called during Startup()
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceDiscoveryDependencies(this IServiceCollection services, ServiceLocationOptions serviceLocationOptions)
        {
            services.AddSingleton<IClientFactory, ClientFactory>();
            services.AddSingleton<ConsulClient>(s => new ConsulClient(c => c.Address = new Uri(serviceLocationOptions.RegistryUrl)));

            services.AddScoped<ILocateService, ServiceLocator>();
            services.AddScoped<IServiceRegistry, ConsulRegistryClient>();
            return services;
        }
    }
}
