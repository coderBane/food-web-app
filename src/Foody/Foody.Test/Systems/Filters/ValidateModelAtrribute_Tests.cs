using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Abstractions;

using Foody.Test.Helpers;
using Foody.Test.Fixtures;
using Foody.WebApi.Filters;


namespace Foody.Test.Systems.Filters
{
    public class ValidateModelAtrribute_Tests
    {
        private readonly ActionContext actionContext;

        public ValidateModelAtrribute_Tests()
        {
            actionContext = new()
            {
                HttpContext = A.Fake<HttpContext>(),
                RouteData = A.Fake<RouteData>(),
                ActionDescriptor = A.Fake<ActionDescriptor>()
            };
        }

        [Fact]
        public void OnActionExecuting_SetResult_BadRequest()
        {
            //Arrange
            var actionArgs = A.Dummy<Dictionary<string, object?>>();
            actionArgs.Add("contact", A.Dummy<ContactDto>());

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                A.Dummy<List<IFilterMetadata>>(),
                actionArgs,
                A.Fake<Controller>());

            var validator = new ObjectValidator();
            var validateModelAttribute = new ValidateModelAttribute();

            //Act
            validator.Validate(actionExecutingContext, null, "contact", actionArgs["contact"]);
            validateModelAttribute.OnActionExecuting(actionExecutingContext);

            //Assert
            Assert.False(actionExecutingContext.ModelState.IsValid);
            Assert.IsType<BadRequestObjectResult>(actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecuting_ModelStateIsValid()
        {
            //Arrange
            var fakeData = InquiriesFixtures.GetContacts();
            var actionArgs = A.Dummy<Dictionary<string, object?>>();
            actionArgs.Add("contact", fakeData);

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                A.Dummy<List<IFilterMetadata>>(),
                actionArgs,
                A.Fake<Controller>());

            var validator = new ObjectValidator();
            var validateModelAttribute = new ValidateModelAttribute();

            //Act
            validator.Validate(actionExecutingContext, null, "contact", actionArgs["contact"]);
            validateModelAttribute.OnActionExecuting(actionExecutingContext);

            //Assert
            Assert.True(actionExecutingContext.ModelState.IsValid);
            Assert.Null(actionExecutingContext.Result);
        }
    }
}

