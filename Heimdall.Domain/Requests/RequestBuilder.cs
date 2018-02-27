using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Heimdall.Gateway.Domain.Requests
{
    public class RequestBuilder : IRequestBuilder
    {
        private HttpMethod _method = null;
        private string _scheme = "http";
        private Uri _uri = null;
        private HttpContent _content = null;
        private string _bearerToken = "";
        private string _acceptHeader = "application/json";
        private IList<Action<HttpRequestHeaders>> _headersToAdd;
        private IHeaderDictionary _headerDictionary;

        public RequestBuilder()
        {
            _headersToAdd = new List<Action<HttpRequestHeaders>>();
        }

        public RequestBuilder AddMethod(HttpMethod method)
        {
            _method = method;
            return this;
        }

        public RequestBuilder AddMethod(string method)
        {
            HttpMethod enumMethod;
            try
            {
                enumMethod = new HttpMethod(method);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Unsupported HttpMethod type {method}", ex);
            }
            return AddMethod(enumMethod);            
        }

        public RequestBuilder AddUri(string resourceIdentifier)
        {
            return AddUri(new Uri(resourceIdentifier));
        }

        public RequestBuilder AddUri(Uri resourceIdentifier)
        {
            _uri = resourceIdentifier;
            return this;
        }

        public RequestBuilder AddScheme(string scheme)
        {
            _scheme = scheme.TrimEnd('/', ':');
            return this;
        }

        public RequestBuilder AddContent(HttpContent httpContent)
        {
            _content = httpContent;
            return this;
        }

        public RequestBuilder AddBearerToken(string bearerToken)
        {
            _bearerToken = bearerToken;
            return this;
        }

        public RequestBuilder AddAcceptHeader(string acceptHeader)
        {
            _acceptHeader = acceptHeader;
            return this;
        }

        public RequestBuilder AddHeader(Action<HttpRequestHeaders> func)
        {
            _headersToAdd.Add(func);
            return this;
        }

        public RequestBuilder AddRequestHeadersToCopy(IHeaderDictionary headerDictionary)
        {
            _headerDictionary = headerDictionary;
            return this;
        }

        public HttpRequestMessage CreateRequest()
        {
            var request = new HttpRequestMessage
            {
                Method = _method,           
                RequestUri = _uri
            };

            if (_content != null)
                request.Content = _content;

            if (!string.IsNullOrEmpty(_bearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            BuildHeaders(request);

            return request;
        }

        private void BuildHeaders(HttpRequestMessage request)
        {
            request.Headers.Clear();

            if (_headerDictionary != null)
            {
                foreach (var key in _headerDictionary.Keys)
                {
                    CopyHeadersToRequestForKey(key, request);
                }
            }

            if (_headersToAdd.Count > 0)
            {
                foreach (var func in _headersToAdd)
                {
                    func(request.Headers);
                }
            }
        }

        private void CopyHeadersToRequestForKey(string key, HttpRequestMessage request)
        {
            if (key.ToUpper() == "HOST" || key.ToUpper().StartsWith("CONTENT"))
                return;

            var value = _headerDictionary[key];
            if (value.Count == 0 || string.IsNullOrEmpty(value[0]))
                return;

            IEnumerable<string> stringValues;
            request.Headers.TryGetValues(key, out stringValues);
            var stringList = stringValues?.ToList() ?? new List<string>();

            if (stringList.Count > 0 && !string.IsNullOrEmpty(stringList[0]))
                return;

            request.Headers.Add(key, value.AsEnumerable());
        }
    }
}
