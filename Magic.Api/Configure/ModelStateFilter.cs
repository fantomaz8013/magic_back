using Magic.Api.Attributes.Magic.Api.Attributes;
using Magic.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Magic.Api.Configure;

public class ModelStateFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;
        var errorMessage = string.Join(Environment.NewLine,
            context.ModelState
                .Values
                .SelectMany(v => v.Errors)
                .Select(m => m.ErrorMessage)
                .ToArray()
        );
        context.Result = new OkObjectResult(ResponseData<string>.Error(errorMessage));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var isCleanResponse = context.ActionDescriptor.EndpointMetadata.Any(x => x is CleanResposeAttribute);
        if (context.Exception != null)
        {
            int? errorCode = null;
            if (context.Exception is ExceptionWithApplicationCode exep)
            {
                errorCode = exep.ErrorCode;
            }

            context.Result = new OkObjectResult(ResponseData<string>.Error(context.Exception.Message, errorCode));
            context.Exception = null;
        }
        else if (context.Result is OkObjectResult result && !isCleanResponse)
        {
            context.Result = new OkObjectResult(ResponseData<object>.Success(result.Value));
        }
    }

    public class ResponseData<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public int? ErrorCode { get; set; }
        public string? ErrorText { get; set; }

        public static ResponseData<T> Success(T data)
        {
            return new ResponseData<T>
            {
                Data = data,
                IsSuccess = true
            };
        }

        public static ResponseData<T> Error(string error, int? errorCode = null)
        {
            return new ResponseData<T>
            {
                IsSuccess = false,
                ErrorText = error,
                ErrorCode = errorCode
            };
        }
    }
}