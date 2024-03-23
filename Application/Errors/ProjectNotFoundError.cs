using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors
{
    public record struct ProjectNotFoundError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string Message => "project not found";
    }
}
