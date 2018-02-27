using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Heimdall.Gateway.Domain.Requests
{
    public interface IRequestBuilder
    {
        RequestBuilder AddAcceptHeader(string acceptHeader);
        RequestBuilder AddHeader(Action<HttpRequestHeaders> func);
        RequestBuilder AddRequestHeadersToCopy(IHeaderDictionary headerDictionary);
        RequestBuilder AddBearerToken(string bearerToken);
        RequestBuilder AddContent(HttpContent httpContent);
        RequestBuilder AddMethod(HttpMethod method);
        RequestBuilder AddMethod(string method);
        //RequestBuilder AddServiceLocation(ServiceEndpoint serviceLocation);
        RequestBuilder AddUri(string resourceIdentifier);
        RequestBuilder AddScheme(string scheme);
        HttpRequestMessage CreateRequest();
    }
}