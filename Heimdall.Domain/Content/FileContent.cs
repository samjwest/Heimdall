using System.IO;
using System.Net.Http;

namespace Heimdall.Gateway.Domain.Content
{
    /// <summary>
    /// Source copied from: https://github.com/TahirNaushad/Fiver.Api.HttpClient
    /// This work was originally completed by Tahir Naushad
    /// and his writeup can be found: https://www.codeproject.com/Articles/1204494/Consuming-ASP-NET-Core-Web-API-using-HttpClient
    /// </summary>
    public class FileContent : MultipartFormDataContent
    {
        public FileContent(string filePath, string apiParamName)
        {
            var filestream = File.Open(filePath, FileMode.Open);
            var filename = Path.GetFileName(filePath);

            Add(new StreamContent(filestream), apiParamName, filename);
        }
    }
}
