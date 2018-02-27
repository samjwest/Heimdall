using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Heimdall.Gateway.Core;
using Heimdall.Gateway.Domain.Requests;

namespace Heimdall.Gateway.Domain.RequestHandlers
{
    public abstract class RequestHandlerBase : IRequestHandler
    {
        protected ILocateService _locationService;
        protected IRequestBuilder _requestBuilder;

        public RequestHandlerBase(ILocateService locator, IRequestBuilder requestBuilder)
        {
            _locationService = locator;
            _requestBuilder = requestBuilder;
        }

        /// <summary>
        /// Override this function if you need steps other than:
        /// 1) Validate
        /// 2) LocateService
        /// 3) BuildRequest
        /// 4) SendRequest
        /// </summary>
        /// <param name="httpContext">the HttpContext of the input service call</param>
        /// <param name="httpContent">the HttpContent of the input service call</param>
        /// <returns>this will return the result of the outgoing service call</returns>
        public virtual async Task<HttpResponseMessage> Process(HttpContext httpContext, HttpContent httpContent)
        {
            //step 1 - Guard Clauses
            Validate(httpContext, httpContent);

            //step 2 - Locate the service - the default will use the service locator
            var targetService = await LocateService(httpContext);
            if (targetService == null)
            {
                //TODO: log the request
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }

            //step 3 - Build Request
            var request = BuildRequest(httpContext, httpContent, targetService.Uri);

            //step 4 - Send Request
            var result = await targetService.SendAsync(request);
            return result;
        }

        public virtual async Task<HttpResponseMessage> Process(HttpContext httpContext)
        {
            return await Process(httpContext, null);
        }

        public virtual void Validate(HttpContext httpContext, HttpContent httpContent)
        {
            if (httpContent == null && httpContext.Request.Method.ToUpper() != "GET") //TODO: delete doesn't have content either...and i am sure there are others
                throw new ArgumentNullException("Content was null when doing a non-Get call");
        }

        public virtual HttpRequestMessage BuildRequest(HttpContext httpContext, HttpContent httpContent, Uri uri)
        {
            AddHeaders(_requestBuilder, httpContext);

            return (_requestBuilder.AddMethod(httpContext.Request.Method)
                            .AddUri(BuildUri(httpContext, uri))
                            .AddContent(httpContent)
                            .CreateRequest());
        }

        public virtual void AddHeaders(IRequestBuilder rb, HttpContext httpContext)
        {
            CopyIncomingRequetHeaders(rb, httpContext);
            AddGatewayHeaders(rb, httpContext);
        }

        public virtual void CopyIncomingRequetHeaders(IRequestBuilder rb, HttpContext httpContext)
        {
            rb.AddRequestHeadersToCopy(httpContext.Request.Headers);
        }

        public virtual void AddGatewayHeaders(IRequestBuilder rb, HttpContext httpContext)
        {
            //override this if you need custom headers
        }

        public virtual Uri BuildUri(HttpContext httpContext, Uri uri)
        {
            return (new UriBuilder(httpContext.Request.Scheme, uri.Host, uri.Port, httpContext.Request.Path, httpContext.Request.QueryString.Value).Uri);
        }

        public virtual async Task<IGatewayService> LocateService(HttpContext httpContext)
        {
            return (await _locationService.Find(httpContext.Request.Path));
        }
    }
}
