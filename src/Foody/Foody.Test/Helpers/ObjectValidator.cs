using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Foody.Test.Helpers
{
    internal sealed class ObjectValidator : IObjectModelValidator
    {
        public void Validate(ActionContext actionContext, ValidationStateDictionary? validationState, string prefix, object? model)
        {
            var results = new List<ValidationResult>();

            var context = new ValidationContext(model, null, null);

            bool isValid = Validator.TryValidateObject(model, context, results, true);

            if (!isValid)
            {
                results.ForEach(err
                    => actionContext.ModelState.AddModelError(err.MemberNames.First() ?? string.Empty, err.ErrorMessage!));
            }
        }
    }
}

