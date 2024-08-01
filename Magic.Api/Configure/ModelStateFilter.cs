using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Magic.Api.Configure
{
    public class ModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                string errorMessage = string.Join(Environment.NewLine, context.ModelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToArray());
                context.Result = new BadRequestObjectResult("Bad request object result");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
