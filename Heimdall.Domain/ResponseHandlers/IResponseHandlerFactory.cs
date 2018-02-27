namespace Heimdall.Gateway.Domain.ResponseHandlers
{
    public interface IResponseHandlerFactory
    {
        IResponseHandler Create(string contentMediaType);
    }
}
