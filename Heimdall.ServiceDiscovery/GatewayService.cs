using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Microsoft.Extensions.Logging;
using Heimdall.Gateway.Registry.Models;
using Heimdall.Gateway.ServiceDiscovery.Clients;
using Heimdall.Gateway.Core.Logging;
using Heimdall.Gateway.Core.Extensions;
using Heimdall.Gateway.Core;

namespace Heimdall.Gateway.ServiceDiscovery
{
    /// <summary>
    /// A GatewayService represents an instance of a service hosted behind the gateway.
    /// </summary>
    public class GatewayService : IGatewayService
    {
        private HttpClient _currentClient;
        private ILogger _logger;

        private List<ServiceEndpoint> _endpoints;
        private IClientFactory _clientFactory;
        private RetryPolicy _serverRetryPolicy;

        
        public GatewayService(IClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _logger = NullLogger.Instance;
        }

        /// <summary>
        /// The target Uri of the requested resource when found during service location.
        /// </summary>
        public Uri Uri { get { return new Uri(_currentClient.BaseAddress, IdentifiedRoute); } }

        public string IdentifiedRoute { get; set; }

        public List<ServiceEndpoint> DiscoveredEndpoints
        {
            get { return _endpoints; }
            set
            {
                _endpoints = value;
                AvailableEndpoints = new Stack<ServiceEndpoint>(_endpoints);
            }
        }

        protected Stack<ServiceEndpoint> AvailableEndpoints { get; set; }

        /// <summary>
        /// Asynchronously pass the request through to the wrapped HttpClient that has been configured for this request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            EnsureConfigured();
            _logger.LogInformation($"Sending {request.Method} request to {Uri}");
            return await _serverRetryPolicy.ExecuteAsync(async () =>
            {
                return await _currentClient.SendAsync(request).ConfigureAwait(false);
            });

        }

        /// <summary>
        /// Select next available service endpoint if one is available
        /// </summary>
        /// <returns></returns>
        public bool TryNext()
        {
            ServiceEndpoint test;
            if (AvailableEndpoints.TryPeek(out test) == false)
            {
                return false;
            }

            _logger.LogWarning($"TryNext() invoked on gateway service for {IdentifiedRoute}");
            _currentClient = BuildNextClient();
            return true;
        }

        /// <summary>
        /// Configure the service to send requests and
        /// Prepare the Retry Policy for fault tollerance.
        /// </summary>
        /// <returns></returns>
        public virtual IGatewayService Configure()
        {
            if (DiscoveredEndpoints.IsNullOrEmpty())
                throw new Exception("InvalidEndpointConfigurationException");

            if (_currentClient == null)
                _currentClient = BuildNextClient();

            IdentifiedRoute = DiscoveredEndpoints[0].Route.Url;
            var retries = AvailableEndpoints.Count;
            _serverRetryPolicy = Policy.Handle<HttpRequestException>()
                   .RetryAsync(retries, (exception, retryCount) =>
                   {
                       TryNext();
                   });

            return this;
        }

        public IGatewayService Configure(ILogger logger)
        {
            _logger = logger;
            return Configure();
        }

        private void EnsureConfigured()
        {
            Configure();
            if(_currentClient == null)
            {
                throw new UnconfigurableClientException();
            }
        }

        private HttpClient BuildNextClient()
        {
            HttpClient nextClient = null;
            ServiceEndpoint test;

            if (AvailableEndpoints.TryPeek(out test) == false)
            {
                throw new NoAvailableServiceEndpointException();
            }

            nextClient = _clientFactory.Create(AvailableEndpoints.Pop());

            return nextClient;
        }
    }
}
