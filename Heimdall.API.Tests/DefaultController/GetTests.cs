using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace Heimdall.Gateway.API.Tests.DefaultController
{
    [TestClass]
    public class GetTests : DefaultControllerTestBase
    {
        [TestMethod]
        public async Task Get_With_NoUrl_NoExtraHeaders_NoPayload_StringResult_OkStatus()
        {
            const string content = "Everything is a OK!";
            var status = System.Net.HttpStatusCode.OK;
            var method = HttpMethod.Get;

            var resultTuple = await TestSetupStringPayload(status, content, method);

            Assert.IsNotNull(resultTuple.Item2);
            Assert.AreEqual(resultTuple.Item1, resultTuple.Item2);

            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpContext>("RequestHandler.Process.HttpContext").Request.Method == method.ToString());

            //this is looking for the payload. the frombody object. this being a get...there won't be one. not sure why it is a string "null" maybe readasstringasync is doing that.
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpContent>("RequestHandler.Process.HttpContent").ReadAsStringAsync().Result == "null");
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpMethod>("RequestHandlerFactory.BuildHandler") == method);
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<int>("ResponseHandler.BuildResult.StatusCode") == (int)status);
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<string>("ResponseHandler.BuildResult.Content") == content);
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<string>("ResponseHandlerFactory.Create") == "text/plain");
        }

        [TestMethod]
        public async Task Get_With_NoUrl_NoExtraHeaders_NoPayload_NoResult_OkStatus()
        {
            string content = null;
            var status = System.Net.HttpStatusCode.OK;
            var method = HttpMethod.Get;

            var resultTuple = await TestSetupStringPayload(status, content, method);

            Assert.IsNotNull(resultTuple.Item2);
            Assert.AreEqual(resultTuple.Item1, resultTuple.Item2);

            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpContext>("RequestHandler.Process.HttpContext").Request.Method == method.ToString());

            //this is looking for the payload. the frombody object. this being a get...there won't be one. not sure why it is a string "null" maybe readasstringasync is doing that.
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpContent>("RequestHandler.Process.HttpContent").ReadAsStringAsync().Result == "null");
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpMethod>("RequestHandlerFactory.BuildHandler") == method);
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<int>("ResponseHandler.BuildResult.StatusCode") == (int)status);
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<string>("ResponseHandler.BuildResult.Content") == "null");
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<string>("ResponseHandlerFactory.Create") == "application/json");
        }

        [TestMethod]
        public async Task Get_With_NoUrl_NoExtraHeaders_NoPayload_StringResult_BadRequestStatus()
        {
            const string content = "Everything is NOT OK!";
            var status = System.Net.HttpStatusCode.BadRequest;
            var method = HttpMethod.Get;

            var resultTuple = await TestSetupStringPayload(status, content, method);

            Assert.IsNotNull(resultTuple.Item2);
            Assert.AreEqual(resultTuple.Item1, resultTuple.Item2);

            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpContext>("RequestHandler.Process.HttpContext").Request.Method == method.ToString());

            //this is looking for the payload. the frombody object. this being a get...there won't be one. not sure why it is a string "null" maybe readasstringasync is doing that.
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpContent>("RequestHandler.Process.HttpContent").ReadAsStringAsync().Result == "null");
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpMethod>("RequestHandlerFactory.BuildHandler") == method);
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<int>("ResponseHandler.BuildResult.StatusCode") == (int)status);
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<string>("ResponseHandler.BuildResult.Content") == content);
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<string>("ResponseHandlerFactory.Create") == "text/plain");
        }

        [TestMethod]
        public async Task Get_With_NoUrl_NoExtraHeaders_NoPayload_NoResult_BadRequestStatus()
        {
            string content = null;
            var status = System.Net.HttpStatusCode.BadRequest;
            var method = HttpMethod.Get;

            var resultTuple = await TestSetupStringPayload(status, content, method);

            Assert.IsNotNull(resultTuple.Item2);
            Assert.AreEqual(resultTuple.Item1, resultTuple.Item2);

            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpContext>("RequestHandler.Process.HttpContext").Request.Method == method.ToString());

            //this is looking for the payload. the frombody object. this being a get...there won't be one. not sure why it is a string "null" maybe readasstringasync is doing that.
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpContent>("RequestHandler.Process.HttpContent").ReadAsStringAsync().Result == "null");
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<HttpMethod>("RequestHandlerFactory.BuildHandler") == method);
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<int>("ResponseHandler.BuildResult.StatusCode") == (int)status);
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<string>("ResponseHandler.BuildResult.Content") == "null");
            Assert.IsTrue(CheckOneExistsAndGetParameterObject<string>("ResponseHandlerFactory.Create") == "application/json");
        }
    }
}
