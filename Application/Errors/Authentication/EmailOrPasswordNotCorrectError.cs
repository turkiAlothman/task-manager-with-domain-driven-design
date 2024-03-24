using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Authentication
{
    public record struct EmailOrPasswordNotCorrectError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public string Message => "username or password is incorrect";
    }
}
