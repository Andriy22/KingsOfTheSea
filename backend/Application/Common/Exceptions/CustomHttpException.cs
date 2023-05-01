using System.Net;

namespace Application.Common.Exceptions;

public class CustomHttpException : Exception
{
    public CustomHttpException(string message, HttpStatusCode code = HttpStatusCode.BadRequest) : base(message)
    {
        StatusCode = code;
    }

    public HttpStatusCode StatusCode { get; set; }
}