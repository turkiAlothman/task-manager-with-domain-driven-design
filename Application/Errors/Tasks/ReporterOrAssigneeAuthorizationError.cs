using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Tasks
{
    public class ReporterOrAssigneeAuthorizationError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
        public string Message => "only the Asignee or the reporter of this task is authorized to edit task";

    }
}
