using System.Net.Http;
using Heimdall.Gateway.Domain.Requests;
using Heimdall.Gateway.Core;

namespace Heimdall.Gateway.Domain.RequestHandlers
{
    public class RequestHandlerFactory : IRequestHandlerFactory
    {
        protected ILocateService _serviceLocator;
        protected IRequestBuilder _requestBuilder;


        public RequestHandlerFactory(ILocateService serviceLocator, IRequestBuilder requestBuilder)
        {
            _serviceLocator = serviceLocator;
            _requestBuilder = requestBuilder;
        }


        /// <summary>
        /// this method will return the correct RequestHandler based on HttpMethod (HttpVerb)
        /// </summary>
        /// <param name="httpVerb">HttpMethod (Get, Post, Delete, Put, ...)</param>
        /// <returns>a Request Handler that can actually create the request and send it to the target service.</returns>
        public IRequestHandler BuildHandler(HttpMethod httpVerb)
        {
            if (httpVerb.Method == HttpMethod.Get.Method)
                return (new GetRequestHandler(_serviceLocator, _requestBuilder)); 

            if (httpVerb.Method == HttpMethod.Post.Method)
                return (new PostRequestHandler(_serviceLocator, _requestBuilder)); 

            if (httpVerb.Method == HttpMethod.Put.Method)
                return (new PutRequestHandler(_serviceLocator, _requestBuilder)); 

            if (httpVerb.Method == HttpMethod.Delete.Method)
                return (new DeleteRequestHandler(_serviceLocator, _requestBuilder)); 

            return (new GetRequestHandler(_serviceLocator, _requestBuilder)); 
        }
    }
}
