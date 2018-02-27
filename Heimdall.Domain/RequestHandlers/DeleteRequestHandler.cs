using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using Heimdall.Gateway.Domain.RequestHandlers;
using Heimdall.Gateway.Domain.Requests;
using Heimdall.Gateway.Core;

namespace Heimdall.Gateway.Domain.RequestHandlers
{
    public class DeleteRequestHandler : RequestHandlerBase
    {
        public DeleteRequestHandler(ILocateService locator, IRequestBuilder requestBuilder) : base(locator, requestBuilder)
        {

        }

        /// <summary>
        /// this could be removed if we updated the base classes Validate to include the delete in the method check.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="httpContent"></param>
        public override void Validate(HttpContext httpContext, HttpContent httpContent)
        {
            if (httpContent == null && httpContext.Request.Method.ToUpper() != "DELETE") //this should never happen. if the method isn't delete in a delete then we have a serious issue.
                throw new ArgumentNullException("Content was null when doing a non-Delete call");
        }
    }
}