using Foody.Test.Fixtures;
using Microsoft.AspNetCore.Mvc;

namespace Foody.Test.Systems.Controllers
{
    public class NewsletterControllerTests : TestBase
    {   
        [Fact]
        public async void Post_Return_NoContent()
        {
            //Arrange
            var subs = Subcribers.GetSubcribers();
            A.CallTo(() => unitofWork.Subcribers.All(string.Empty)).Returns(Task.FromResult(subs));
            var controller = new NewsletterController(unitofWork, mapper, cacheService);
            var fakeSub = A.Dummy<NewsletterDto>();
            fakeSub.Email = subs.First().Email;
            fakeSub.Name = subs.First().Name;

            //Act
            var actionResult = await controller.Post(fakeSub);

            //Assert
            var result = Assert.IsType<NoContentResult>(actionResult);
        }
    }
}

