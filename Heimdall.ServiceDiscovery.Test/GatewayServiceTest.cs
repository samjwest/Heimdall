using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Heimdall.Gateway.Registry.Models;
using Heimdall.Gateway.ServiceDiscovery.Clients;

namespace Heimdall.Gateway.ServiceDiscovery.Tests
{
    [TestClass]
    public class GatewayServiceTest
    {
        protected Mock<HttpClient> _mockClient;
        protected Mock<IClientFactory> _mockClientFactory;
        //protected Mock<ServiceEndpoint> _mockEndpoint;
        protected List<ServiceEndpoint> _serviceEndpoints;

        [TestInitialize]
        public void Initialize()
        {
            _mockClientFactory = new Mock<IClientFactory>();
            _mockClient = new Mock<HttpClient>();
            _mockClientFactory.Setup(x => x.Create(It.IsAny<ServiceEndpoint>())).Returns(_mockClient.Object);

            _serviceEndpoints = new List<ServiceEndpoint>()
            {
                new ServiceEndpoint()
                {
                    Host = new Host { Hostname = "localhost", Port = 64536 },
                    Route = new Route("api/resource/operator"),
                    Prefix = ""
                }
            };

        }

        [TestMethod]
        public void IdentifiedRoute_ShouldReturnNullWhenEndpointNotIncluded()
        {
            var result = new GatewayService(_mockClientFactory.Object); 

            result.IdentifiedRoute.Should().BeNull();
        }

        [TestMethod]
        public void IdentifiedRoute_ShouldReturnMatchingRouteWithEndpointIncluded()
        {
            var result = new GatewayService(_mockClientFactory.Object)
                .WithEndpoints(_serviceEndpoints)
                .Configure();

            result.IdentifiedRoute.Should().BeEquivalentTo(_serviceEndpoints[0].Route.Url);
        }

        //[TestMethod]
        //public async Task SendAsync_ShouldReturnTaskResponseForGivenRequest()
        //{
        //    var result = await 
        //}
    }
}
