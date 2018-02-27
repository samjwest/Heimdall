using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Logging;
using Heimdall.Gateway.Registry;
using Heimdall.Gateway.ServiceDiscovery.Clients;
using Heimdall.Gateway.Core.Extensions;
using Heimdall.Gateway.Core;

namespace Heimdall.Gateway.ServiceDiscovery
{
    public class ServiceLocator : ILocateService
    {
        private IClientFactory _clientFactory;
        private IServiceRegistry _serviceRegistry;
        private ILoggerFactory _loggerFactory;
        private ILogger _logger;

        private readonly string[] IGNORED_TOKENS = { "api", }; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientFactory"></param>
        public ServiceLocator(IClientFactory clientFactory, IServiceRegistry serviceRegistry, ILoggerFactory loggerFactory)
        {
            _clientFactory = clientFactory;
            _serviceRegistry = serviceRegistry;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<ServiceLocator>();
        }

        /// <summary>
        /// Find the existance of a service that is available to process the given route.
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public Task<IGatewayService> Find(string route)
        {
            return Find("Default", route);
        }

        /// <summary>
        /// Find the existance of a service that is available to process the given route.
        /// Optionally, accept a qualifying domain used to segregate underlying services into specialized domains.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public async Task<IGatewayService> Find(string domain, string route)
        {
            if (string.IsNullOrEmpty(route))
                return null;

            _logger.LogInformation($"Request for route [{route}] with domain = {domain} received.");

            //TODO: Deal with domains later
            if (route.StartsWith('/'))                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
                route = route.Substring(1);

            var tokens = ExtractTokens(route);

            try
            {
                // Plug in Registry and lookup route - should return a list of endpoints registered for the given token list
                var endpoints = await _serviceRegistry.FindByTags(tokens);
                if (endpoints == null || endpoints.Count == 0)
                    return null;

                var svcLogger = _loggerFactory.CreateLogger<IGatewayService>();
                var service = new GatewayService(_clientFactory)
                    .WithEndpoints(endpoints)
                    .WithRoute(route)
                    .Configure(svcLogger);

                _logger.LogInformation($"{endpoints.Count} Services matching route found");
                return service;
            }
            catch (Exception ex)
            {
                string errMsg = $"Error occured while finding {route}";
                _logger.LogError(errMsg, ex);

                if (ex is RegistryUnavailableException)
                {
                    errMsg = "Service Registry is not responding";
                    _logger.LogError(errMsg);
                }

            }

            return null;           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        private List<string> ExtractTokens(string route)
        {
            List<string> tokens = new List<string>();

            string[] parts = route.Split('/');
            foreach(string part in parts)
            {
                if (string.IsNullOrEmpty(part) || part.ToLower().In(IGNORED_TOKENS))
                    continue;

                if (part.IsNumeric() || part.IsGuid())
                    continue;

                tokens.Add(part.ToUpper());
            }

            return tokens;
        }

    }
}
