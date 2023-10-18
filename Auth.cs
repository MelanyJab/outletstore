using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using outletstore.Controllers;

namespace outletstore.Functions
{
    public class ApiAuthentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string token = actionContext.Request.Headers.Authorization?.Parameter ?? "";
            Crypto userauth = new Crypto();
            if (!userauth.IsTokenValid(token, ref actionContext))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, "UnAuthorized!");
                return;
            }
            base.OnActionExecuting(actionContext);
        }
    }
}

