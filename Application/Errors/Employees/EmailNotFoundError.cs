using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Employees
{
    public  record struct EmailNotFoundError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
        public string Message => "Email not found";
    }
}
