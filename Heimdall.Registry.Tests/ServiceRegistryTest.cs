using Consul;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heimdall.Gateway.Registry.Clients;

namespace Heimdall.Gateway.Registry.Tests
{
    [TestClass]
    public class ServiceRegistryTest
    {
        protected IServiceRegistry _serviceRegistry;
        protected Mock<ConsulClient> _mockConsulClient;
        protected QueryResult<Dictionary<string,AgentService>> _serviceResult;

        public ServiceRegistryTest()
        {
            _mockConsulClient = new Mock<ConsulClient>();            
            _serviceRegistry = new ConsulRegistryClient(_mockConsulClient.Object);
        }

        [TestInitialize]
        public void Initialize()
        {            
            var dictionary = new Dictionary<string, AgentService>();
            dictionary.Add("TruckService", new AgentService()
            {
                ID = "",
                Address = "",
                Port = 8100,
                Tags = new string[]{ "truck", "dodge", "chevy" }
            });
            dictionary.Add("DodgeService ", new AgentService()
            {
                ID = "",
                Address = "",
                Port = 8200,
                Tags = new string[] { "truck", "dodge" }
            });

            _serviceResult = new QueryResult<Dictionary<string, AgentService>>(new QueryResult(), dictionary);


            //Mock<IAgentEndpoint> agent = new Mock<IAgentEndpoint>();                     .
            //agent.Setup(x => x.Services()).ReturnsAsync(_serviceResult);
            //_mockConsulClient = new Mock<ConsulClient>();
            //_mockConsulClient.Setup(x => x.Agent).Returns(agent.Object);            
            //_serviceRegistry = new ConsulRegistryClient(_mockConsulClient.Object);
        }

        [TestMethod]
        public async Task FindByTags_ShouldReturnNullWhenTagsAreNull()
        {
            var result = await _serviceRegistry.FindByTags(null);
            result.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task FindByTags_ShouldReturnNullWhenTagsAreEmpty()
        {
            List<string> tags = new List<string> { };
            var result = await _serviceRegistry.FindByTags(tags);
            result.Count.Should().Be(0);
        }


        // Can't mock awaitable Services dictionary with optional param
        //[TestMethod]
        //public async Task FindByTags_ShouldReturnSingleServiceWhenOneTagMatched()
        //{
        //    List<string> tags = new List<string> { "chevy" };
        //    var result = await _serviceRegistry.FindByTags(tags);
        //    result.Count.Should().Be(1);
        //    result.First().Name.Should().Be("TruckService");
        //}
    }
}
