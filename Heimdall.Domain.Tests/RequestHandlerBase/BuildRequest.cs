using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Heimdall.Gateway.Domain.RequestHandlers;
using Moq;
using Heimdall.Gateway.Core;
using Heimdall.Gateway.Domain.Requests;
using Microsoft.AspNetCore.Http;

namespace Heimdall.Gateway.Domain.Tests.RequestHandlerBase
{
    [TestClass]
    public class BuildRequest
    {
        [TestMethod]
        public async Task TestAddHeaders()
        {
            var requestBuilder = new RequestBuilder();
            var getRequestHandler = new GetRequestHandler(new Mock<ILocateService>().Object, requestBuilder);

            getRequestHandler.AddHeaders(requestBuilder, null);
        }

        [TestMethod]
        public async Task Process()
        {
            var gatewayService = new Mock<IGatewayService>();
            var serviceLocator = new Mock<ILocateService>();
            var requestBuilder = new RequestBuilder();

            gatewayService.Setup(func => func.Uri).Returns(new System.Uri("http://192.168.1.100:12345/MyTestUri"));
            serviceLocator.Setup(func => func.Find(It.IsAny<string>())).Returns(Task.FromResult(gatewayService.Object));
            var getRequestHandler = new GetRequestHandler(serviceLocator.Object, requestBuilder);

            var httpcontext = new DefaultHttpContext();

            httpcontext.Request.Method = HttpMethods.Get;
            httpcontext.Request.Scheme = "http";
            httpcontext.Request.Path = new PathString("/api");
            var responseMessage = await getRequestHandler.Process(httpcontext);
        }
    }
}