using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Heimdall.Gateway.Registry.Models;
using Heimdall.Gateway.ServiceDiscovery.Clients;

namespace Heimdall.Gateway.ServiceDiscovery.Tests
{
    [TestClass]
    public class ClientFactoryTest
    {
        protected Mock<HttpClient> _mockHttpClient;
        protected IClientFactory _clientFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockHttpClient = new Mock<HttpClient>();
            _clientFactory = new ClientFactory();
        }

        [TestMethod]
        public void Create_ShouldReturnNullWhenServiceEndpointIsNull()
        {
            ServiceEndpoint endpoint = null;
            var result = _clientFactory.Create(endpoint);

            result.Should().BeNull();
        }

        [TestMethod]
        public void Create_ShouldReturnClientWithGivenEndpoint()
        {
            ServiceEndpoint endpoint = new ServiceEndpoint()
            {
                Host = new Host { Hostname = "localhost", Port = 64536 },
                Route = new Route("api/resource/operator"),
                Scheme = "Http",
                Prefix = ""
            };

            var result = _clientFactory.Create(endpoint);
            result.Should().NotBeNull();
            result.BaseAddress.Should().BeEquivalentTo(endpoint.BaseAddress);

        }
    }
}
