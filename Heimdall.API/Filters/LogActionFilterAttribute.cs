using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Heimdall.Gateway.API.Filters
{
    public class LogActionAttribute : TypeFilterAttribute
    {
        public LogActionAttribute():base(typeof(LogActionFilter)) { }

        private class LogActionFilter : IActionFilter
        {
            private ILogger _logger;
            public LogActionFilter(ILoggerFactory loggerFactory)
            {
                _logger = loggerFactory.CreateLogger<LogActionFilter>();
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {                
                string msg = GetLogMessage(context.HttpContext, context.RouteData);
                _logger.LogDebug($"OnActionExecuting {msg}");
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                string msg = GetLogMessage(context.HttpContext, context.RouteData);
                _logger.LogDebug($"OnActionExecuted {msg}");
            }


            private string GetLogMessage(HttpContext context, RouteData routeData)
            {
                IPAddress remoteAddr = context.Connection.RemoteIpAddress;
                string routing = GetroutingMessage(routeData, context.Request.Method);
                string url = context.Request.Path;
                
                return $"[From {remoteAddr}] {routing} Url:{url}";
            }

            private string GetroutingMessage(RouteData routeData, string verb)
            {
                var controllerName = routeData.Values["controller"];
                var actionName = routeData.Values["action"];
                var message = $"{controllerName}Controller:{verb}-{actionName}";
                return message;
            }
        }

    }
}
