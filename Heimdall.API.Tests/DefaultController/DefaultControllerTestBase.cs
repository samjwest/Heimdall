using Moq;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Heimdall.Gateway.Domain.Content;
using Heimdall.Gateway.Domain.RequestHandlers;
using Heimdall.Gateway.Domain.ResponseHandlers;

namespace Heimdall.Gateway.API.Tests.DefaultController
{
    public abstract class DefaultControllerTestBase
    {
        protected Mock<ILoggerFactory> _loggerFactory;
        protected Mock<IRequestHandlerFactory> _requestHandlerFactory;
        protected Mock<IRequestHandler> _requestHandler;
        protected Mock<IResponseHandlerFactory> _responseHandlerFactory;
        protected Mock<IResponseHandler> _responseHandler;
        protected Dictionary<string, List<object>> FunctionParameterDictionary;

        public DefaultControllerTestBase()
        {
            _loggerFactory = new Mock<ILoggerFactory>();
            _requestHandlerFactory = new Mock<IRequestHandlerFactory>();
            _requestHandler = new Mock<IRequestHandler>();
            _responseHandlerFactory = new Mock<IResponseHandlerFactory>();
            _responseHandler = new Mock<IResponseHandler>();
            FunctionParameterDictionary = new Dictionary<string, List<object>>();
        }

        #region RequestHandlerHelpers

        protected virtual void BuildAndFinalizeRequestHandlerFactory<T>(HttpStatusCode httpStatusCode, T content)
        {
            FinalizeRequestHandlerFactory(CreateHttpResponseMessage(httpStatusCode, CreateStringContent(content)));
        }

        protected virtual HttpContent CreateStringContent<T>(T content)
        {
            if (content is string)
                return (new StringContent(content as string));
            else
                return (new JsonContent(content));
        }

        protected virtual HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode httpStatusCode, HttpContent httpContent)
        {
            var response = new HttpResponseMessage(httpStatusCode);
            response.Content = httpContent;
            return (response);
        }

        protected virtual void FinalizeRequestHandlerFactory(HttpResponseMessage responseMessage)
        {
            _requestHandler.Setup<Task<HttpResponseMessage>>(func => func.Process(It.IsAny<HttpContext>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(responseMessage))
                .Callback((HttpContext hcx, HttpContent hcn) => {
                                                                    AddToFunctionParameterDictionary("RequestHandler.Process.HttpContext", hcx);
                                                                    AddToFunctionParameterDictionary("RequestHandler.Process.HttpContent", hcn);
                                                                });
            _requestHandlerFactory.Setup<IRequestHandler>(func => func.BuildHandler(It.IsAny<HttpMethod>()))
                .Returns(_requestHandler.Object)
                .Callback((HttpMethod hm) => AddToFunctionParameterDictionary("RequestHandlerFactory.BuildHandler", hm));
        }

        #endregion

        #region ResponseHandlerHelpers

        protected virtual IActionResult BuildAndFinalizeResponseHandlerFactory<T>(T content)
        {
            var contentResult = CreateContentResult(content);
            FinalizeResponseHandlerFactory(contentResult);

            return (contentResult);
        }

        protected virtual IActionResult CreateContentResult<T>(T content)
        {
            if (content is string)
            {
                var contentResult = new ContentResult();
                contentResult.Content = content as string;
                return (contentResult);
            }

            var objectResult = new ObjectResult(content);
            return (objectResult);
        }

        protected virtual void FinalizeResponseHandlerFactory(IActionResult expectedResult)
        {
            _responseHandler.Setup<IActionResult>(func => func.BuildResult(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(expectedResult)
                .Callback((int i, object o) => {
                    AddToFunctionParameterDictionary("ResponseHandler.BuildResult.StatusCode", i);
                    AddToFunctionParameterDictionary("ResponseHandler.BuildResult.Content", o);
                });
            _responseHandlerFactory.Setup<IResponseHandler>(func => func.Create(It.IsAny<string>()))
                .Returns(_responseHandler.Object)
                .Callback((string mediaType) => AddToFunctionParameterDictionary("ResponseHandlerFactory.Create", mediaType));
        }

        #endregion

        #region LoggerHelpers

        protected virtual void FinalizeLoggerFactory()
        {
            var logger = new Mock<ILogger>();
            _loggerFactory.Setup<ILogger>(func => func.CreateLogger(It.IsAny<string>()))
                .Returns(logger.Object);
        }

        #endregion

        #region ControllerHelpers

        protected virtual Controllers.DefaultController FinalizeController(HttpMethod httpMethod)
        {
            FinalizeLoggerFactory();

            var controller = new Controllers.DefaultController(_loggerFactory.Object, _requestHandlerFactory.Object, _responseHandlerFactory.Object);

            controller.ControllerContext = BuildControllerContext(httpMethod);

            return (controller);
        }

        protected virtual ControllerContext BuildControllerContext(HttpMethod httpMethod)
        {
            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = new DefaultHttpContext();
            controllerContext.HttpContext.Request.Method = httpMethod.Method;

            return (controllerContext);
        }

        #endregion

        #region PublicHelpers

        protected async Task<Tuple<IActionResult, IActionResult>> TestSetupStringPayload<T>(HttpStatusCode status, T returnedPayload, HttpMethod method)
        {
            BuildAndFinalizeRequestHandlerFactory(status, returnedPayload);
            var expectedResult = BuildAndFinalizeResponseHandlerFactory(returnedPayload);
            var controller = FinalizeController(method);
            return(new Tuple<IActionResult, IActionResult>(expectedResult, await controller.Get()));
        }

        protected T CheckOneExistsAndGetParameterObject<T>(string key, int index = 0)
        {
            Assert.IsTrue(FunctionParameterDictionary[key].Count == 1);
            return (GetParameterObject<T>(key, index));
        }

        protected T GetParameterObject<T>(string key, int index = 0)
        {
            return ((T)FunctionParameterDictionary[key][index]);
        }

        #endregion

        #region Privatehelpers

        private void AddToFunctionParameterDictionary(string functionCall, object parameter)
        {
            if (FunctionParameterDictionary.ContainsKey(functionCall))
                FunctionParameterDictionary[functionCall].Add(parameter);
            else
                FunctionParameterDictionary.Add(functionCall, new List<object>() { parameter });
        }

        #endregion
    }
}
