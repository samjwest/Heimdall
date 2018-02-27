namespace Heimdall.Gateway.Domain.ResponseHandlers
{
    public class ResponseHandlerFactory : IResponseHandlerFactory
    {
        public IResponseHandler Create(string contentMediaType)
        {
            switch (contentMediaType)
            {
                case "text/plain":
                    return (new StringResponseHandler());
                case "application/json":
                    return (new JSONResponseHandler());
                case "":
                    return (new NoContentResponseHandler());
                default:
                    return (new ObjectResponseHandler());
            }
        }
    }
}
