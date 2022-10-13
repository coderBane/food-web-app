using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foody.WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private static readonly Result<dynamic> _result = new();

    public ExceptionFilter()
    {
        _result.Error = new()
        {
            Code = 500,
            Title = "Server Error",
            Message = ErrorsMessage.Generic.UnknownError
        };
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is Exception)
        {
            context.Result = new ObjectResult(_result)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };

            context.ExceptionHandled = true;
        }
    }
}

