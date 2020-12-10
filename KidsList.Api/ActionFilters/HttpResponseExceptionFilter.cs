using KidsList.Api.Exceptions;
using KidsList.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KidsList.Api.ActionFilters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = 400
                };
                context.ExceptionHandled = true;

                return;
            }

            if (context.Exception is UserMissingException userMissingException)
            {
                context.Result = new ObjectResult(userMissingException.Message)
                {
                    StatusCode = 401
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
