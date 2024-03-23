using System.Net;

namespace Application.Errors
{
    public interface IError
    {
        public HttpStatusCode StatusCode {  get; }
        public string Message { get; }
    }
}
