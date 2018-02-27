using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Heimdall.Gateway.Domain.Requests;
using Heimdall.Gateway.Domain.RequestHandlers;
using Heimdall.Gateway.Domain.ResponseHandlers;

namespace Heimdall.Gateway.Domain
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandlerFactory, RequestHandlerFactory>();
            services.AddScoped<IResponseHandlerFactory, ResponseHandlerFactory>();
            services.AddScoped<IRequestBuilder, RequestBuilder>();
            return services;
        }
    }
}
