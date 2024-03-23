using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Tasks
{
    public record struct UnathorizedTaskActionError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
        public string Message => "only the reporter who allowed to do this action";
    }
}
