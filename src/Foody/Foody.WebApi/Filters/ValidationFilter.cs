using Foody.WebApi.Controllers.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foody.WebApi.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            dynamic result;

            var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage).AsParallel().ToList();

            if (context.Controller.GetType().IsSubclassOf(typeof(AuthBaseController)))
            {
                result = new AuthResult
                {
                    Success = false,
                    Errors = errors
                };
            }
            else
            {
                result = new Result<dynamic>
                {
                    Error = new()
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Title = ErrorsMessage.Generic.ValidationError,
                        Message = errors.Aggregate((k, v) => $"{k} {Environment.NewLine} {v}")
                    }
                };
            }

            context.Result = new BadRequestObjectResult(result);
        }
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {

    }
}

