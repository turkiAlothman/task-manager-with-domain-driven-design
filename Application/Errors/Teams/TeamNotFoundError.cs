using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Teams
{
    public record struct TeamNotFoundError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public string Message => "team not found";
    }
}
