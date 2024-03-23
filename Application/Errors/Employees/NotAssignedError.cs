using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Employees
{
    public  record struct NotAssignedError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public string Message => "Employee is not assigned to this task";
    }
}
