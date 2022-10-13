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
                        Title = ErrorsMessage.Generic.BadRequest,
                        Message = errors.Aggregate((k, v) => $"{k} {Environment.NewLine} {v}")
                    }
                };
            }

            context.Result = new BadRequestObjectResult(result);
        }
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class NullableAttribute : ActionFilterAttribute
{
    private readonly Func<Dictionary<string, object?>, bool> _condition;

    public NullableAttribute(Func<Dictionary<string, object?>, bool> condition)
    {
        _condition = condition;
    }

    public NullableAttribute() : this(args => args.ContainsValue(null)) { }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (_condition((Dictionary<string, object?>)context.ActionArguments))
        {
            var result = new Result<dynamic>
            {
                Error = new()
                {
                    Code = StatusCodes.Status400BadRequest,
                    Title = ErrorsMessage.Generic.InvalidRequest
                }
            };

            context.Result = new BadRequestObjectResult(result);
        }
    }
}

