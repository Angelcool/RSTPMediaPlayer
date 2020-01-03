using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace VideoPlayerForWpf
{
    /// <summary>
    /// ModelValidationFilterAttribute
    /// </summary>
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.ModelState.IsValid == false)
                {
                    // Return the validation errors in the response body.
                    // 在响应体中返回验证错误
                    var errors = new Dictionary<string, IEnumerable<string>>();
                    foreach (KeyValuePair<string, ModelState> keyValue in actionContext.ModelState)
                    {
                        errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
                    }

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                }
            }
            catch (Exception ex)
            {
                //CommonHelper.WriteLog(ex);
            }
        }
    }
}
