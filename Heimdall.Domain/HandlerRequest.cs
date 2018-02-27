using Microsoft.AspNetCore.Http;

namespace Heimdall.Gateway.Domain
{
    public class HandlerRequest
    {
        public string Controller { get; private set; }

        public string ActionName { get; private set; }

        public string HandlerName { get; private set; }

        public string HTTPVerb { get { return (_context.Request.Method); } }

        public string Url { get { return ($"{_context.Request.Scheme}://{_context.Request.Host}{_context.Request.Path}{_context.Request.QueryString}"); } }

        public string QueryString { get { return (_context.Request.QueryString.Value); } }

        public object RequestBody { get; private set; }

        private HttpContext _context { get; set; }

        public HandlerRequest(HttpContext Context, string actionName, string handlerName, /*string url,*/ object requestBody = null, string controller = "Default")
        {
            _context = Context;
            ActionName = actionName;
            HandlerName = handlerName;
            Controller = controller;
            RequestBody = requestBody;
        }
    }
}