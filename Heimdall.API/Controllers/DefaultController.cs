using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Heimdall.Gateway.Domain;
using Heimdall.Gateway.Domain.RequestHandlers;
using Heimdall.Gateway.Domain.Response;
using Heimdall.Gateway.API.Filters;
using Heimdall.Gateway.Domain.Content;
using Heimdall.Gateway.Domain.ResponseHandlers;
using Microsoft.AspNetCore.Authorization;

namespace Heimdall.Gateway.API.Controllers
{
    /// <summary>
    /// The default route maps to this controller which is responsible for matching 
    /// the requested resource and operation to a backend service endpoint.
    /// </summary>
    [LogAction]
    [Produces("application/json")]
    public class DefaultController : Controller
    {
        private const string _HandlerName = "Handler";

        private ILogger _logger;
        private Guid _requestID;

        private IRequestHandlerFactory _requestHandlerFactory;
        private IResponseHandlerFactory _responseHandlerFactory;

        public DefaultController(ILoggerFactory LoggerFactory, IRequestHandlerFactory requestHandlerFactory, IResponseHandlerFactory responseHandlerFactory)
        {
            _logger = LoggerFactory.CreateLogger<DefaultController>();
            _requestID = Guid.NewGuid();
            _requestHandlerFactory = requestHandlerFactory;
            _responseHandlerFactory = responseHandlerFactory;
        }

        /// <summary>
        /// Default action for HttpGet requests received by the gateway.
        /// </summary>
        /// <param name="*"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        //[Authorize]
        [ActionName(_HandlerName)]
        public async Task<IActionResult> Get(/*string url*/)
        {            
            return (await HandleRequest(HttpMethod.Get, /*url,*/ null));
        }

        /// <summary>
        /// Default action for HttpPost requests received by the gateway.
        /// </summary>
        /// <param name="*"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName(_HandlerName)]
        public async Task<IActionResult> Post(/*string url,*/ [FromBody]object value)
        {
            return (await HandleRequest(HttpMethod.Post, /*url,*/ value));
        }

        /// <summary>
        /// Default action for HttpPut requests received by the gateway.
        /// </summary>
        /// <param name="*"></param>
        /// <returns></returns>
        [HttpPut]
        [ActionName(_HandlerName)]
        public async Task<IActionResult> Put(/*string url,*/ [FromBody]object value)
        {
            return (await HandleRequest(HttpMethod.Put, /*url,*/ value));
        }

        /// <summary>
        /// Default action for HttpDelete requests received by the gateway.
        /// </summary>
        /// <param name="*"></param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName(_HandlerName)]
        public async Task<IActionResult> Delete(/*string url*/)
        {
            return (await HandleRequest(HttpMethod.Delete, /*url,*/ null));
        }

        private async Task<IActionResult> HandleRequest(HttpMethod httpVerb, /*string url,*/ object value)
        {
            var request = new HandlerRequest(HttpContext, _HandlerName, MethodBase.GetCurrentMethod().Name, /*url,*/ value);
            _logger.LogDebug($"{_requestID} - HandlerRequest: {JsonConvert.SerializeObject(request)}");

            var handler = _requestHandlerFactory.BuildHandler(httpVerb);
            var result = await handler.Process(HttpContext, new JsonContent(value));

            return (FormatResult(result));
        }

        private IActionResult FormatResult(HttpResponseMessage result)
        {
            if (result.StatusCode == HttpStatusCode.ServiceUnavailable)
                return NotFound(result.RequestMessage);
            
            var stringResult = result.Content.AsString();

            _logger.LogDebug($"{_requestID} - Result StatusCode From remote API call ID: {(int)result.StatusCode} - Codename: {result.StatusCode}");
            _logger.LogDebug($"{_requestID} - StringResult From remote API call: {stringResult}");

            string mediaType = result.Content.Headers?.ContentType?.MediaType ?? string.Empty;
            var responseHandler = _responseHandlerFactory.Create(mediaType);

            return responseHandler.BuildResult((int)result.StatusCode, stringResult);            
        }
    }
}
