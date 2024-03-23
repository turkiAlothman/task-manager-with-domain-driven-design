using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Authorizations
{
    public class ManagerAuthorizationError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public string Message => "only the manager is authorized to do this action";
    }
}
