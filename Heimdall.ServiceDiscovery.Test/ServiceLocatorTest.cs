using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;
using System.Net.Http;
using System.Threading.Tasks;
using Heimdall.Gateway.Registry.Models;
using Heimdall.Gateway.ServiceDiscovery.Clients;
using Heimdall.Gateway.Core;
using Heimdall.Gateway.Registry;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Heimdall.Gateway.ServiceDiscovery.Tests
{
    [TestClass]
    public class ServiceLocatorTest
    {
        protected Mock<IClientFactory> _mockClientFactory;
        protected Mock<HttpClient> _mockClient;
        protected Mock<IServiceRegistry> _mockServiceRegistry;
        protected Mock<ILoggerFactory> _mockLoggerFactory;
        protected ILocateService _serviceLocator;

        public ServiceLocatorTest()
        {
            _mockClientFactory = new Mock<IClientFactory>();
            _mockClient = new Mock<HttpClient>();
            _mockLoggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            _mockLoggerFactory.Setup<ILogger>(func => func.CreateLogger(It.IsAny<string>()))
                .Returns(logger.Object);
            
            _mockClientFactory.Setup(x => x.Create(It.IsAny<ServiceEndpoint>())).Returns(_mockClient.Object);

            var endpoints = new List<ServiceEndpoint>()
            {
                new ServiceEndpoint(){ Host = new Host("localhost", 80), Route = new Route("api/resource/operator") }
            };
            
            _mockServiceRegistry = new Mock<IServiceRegistry>();
            _mockServiceRegistry.Setup(x => x.FindByTags(It.IsAny<List<string>>())).ReturnsAsync(endpoints);

            _serviceLocator = new ServiceLocator(_mockClientFactory.Object, _mockServiceRegistry.Object, _mockLoggerFactory.Object);
        }

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public async Task Find_ShouldReturnNullWhenRouteIsNull()
        {
            var result = await _serviceLocator.Find(null);

            result.Should().BeNull();
        }

        [TestMethod]
        public async Task Find_ShouldReturnServiceWithUriMatchingGivenRoute()
        {
            string testRoute = "api/resource/operator";

            var result = await _serviceLocator.Find(testRoute);

            result.IdentifiedRoute.Should().BeEquivalentTo(testRoute);
        }
    }
}
