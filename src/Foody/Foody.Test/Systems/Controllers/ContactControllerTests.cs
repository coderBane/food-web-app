using Foody.Test.Fixtures;

namespace Foody.Test.Systems.Controllers
{
    public class ContactControllerTests : TestBase
    {
        const string cache = "inquiries";
        readonly string search = string.Empty;

        [Fact]
        public async void Get_All_ReturnOk()
        {
            //Arrange
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(string.Empty);

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public async void Get_All_InvokeUnitOfWork_ReturnOk()
        {
            //Arrange
            A.CallTo(() => cacheService.GetData<IEnumerable<Contact>>(cache))
                .Returns(Task.FromResult<IEnumerable<Contact>?>(null));
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(search);

            //Assert
            A.CallTo(() => unitofWork.Inquiries.AllAsync(search)).MustHaveHappenedOnceExactly();
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public async void Get_All_FromCache_ReturnOk()
        {
            //Arrange
            var fakeData = A.CollectionOfDummy<Contact>(5).AsEnumerable();
            A.CallTo(() => cacheService.GetData<IEnumerable<Contact>>(cache)).Returns(Task.FromResult(fakeData));
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(search);

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<Pagination<Contact>>(result.Value);
            Assert.True(data.Success);
            Assert.NotNull(data.Content);
            Assert.Equal(5, data.Content.Count());
        }

        [Fact]
        public async void Get_All_FromDb_ReturnOK()
        {
            // Arrange
            var fakeData = A.CollectionOfDummy<Contact>(5).AsEnumerable();
            A.CallTo(() => cacheService.GetData<IEnumerable<Contact>>(cache))
                .Returns(Task.FromResult<IEnumerable<Contact>?>(null));
            A.CallTo(() => unitofWork.Inquiries.AllAsync(search)).Returns(Task.FromResult(fakeData));
            var controller = new ContactController(unitofWork, mapper, cacheService);

            // Act
            var actionResult = await controller.Get(default(string));

            // Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<Pagination<Contact>>(result.Value);
            Assert.Equal(5, data.ResultItems);
        }

        [Fact]
        public async void Get_Single_ReturnOK()
        {
            //Arrange
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(0);

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public async void Get_Single_InvokeUnitOfWork_ReturnOk()
        {
            //Arrange
            const int id = 2;
            A.CallTo(() => cacheService.GetData<Contact>($"{cache}-{id}")).Returns(Task.FromResult<Contact?>(null));

            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(id);

            //Assert
            A.CallTo(() => unitofWork.Inquiries.GetAsync(id)).MustHaveHappenedOnceExactly();
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Theory]
        [InlineData(213)]
        [InlineData(215)]
        [InlineData(217)]
        public async void Get_Single_FromDb_ReturnOk(int id)
        {
            //Arrange
            var fakeData = InquiriesFixtures.GetContacts();
            A.CallTo(() => cacheService.GetData<Contact>($"{cache}-{id}")).Returns(Task.FromResult((Contact?)null));
            A.CallTo(() => unitofWork.Inquiries.GetAsync(id))
                .Returns(Task.FromResult(fakeData.FirstOrDefault(x => x.Id == id)));
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(id);

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<Result<Contact>>(result.Value);
            Assert.True(data.Success);
            Assert.NotNull(data.Content);
            Assert.Equal(id, data.Content.Id);
        }

        [Fact]
        public async Task Get_Single_ReturnNotFound()
        {
            //Arrange
            const int id = 300;
            var fakeData = InquiriesFixtures.GetContacts();
            A.CallTo(() => cacheService.GetData<Contact>($"{cache}-{id}")).Returns(Task.FromResult((Contact?)null));
            A.CallTo(() => unitofWork.Inquiries.GetAsync(id))
                .Returns(Task.FromResult<Contact?>(null));
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(id);

            //Assert
            var result = Assert.IsType<NotFoundObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<Result<Contact>>(result.Value);
            Assert.False(data.Success);
            Assert.NotNull(data.Error);
            Assert.Null(data.Content);
        }

        [Fact]
        public async Task ToggleRead_Return_NoContent()
        {
            //Arrange
            A.CallTo(() => unitofWork.Inquiries.ToggleRead(2)).Returns(Task.FromResult(true));
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.ToggleRead(2);

            //Assert
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        public async Task ToggleRead_Return_NotFound()
        {
            //Arrange
            A.CallTo(() => unitofWork.Inquiries.ToggleRead(2)).Returns(Task.FromResult(false));
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.ToggleRead(2);

            //Assert
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        //[Fact]
        //public async Task Get_Inquiry_ReturnServerError()
        //{
        //    //Arrange
        //    A.CallTo(() => cacheService.GetData<Contact>(key)).Returns(Task.FromResult((Contact?)null));
        //    A.CallTo(() => unitofWork.Contacts.Get(id)).ThrowsAsync(() => new Exception("Exception Occurred."));
        //    var controller = new ContactController(unitofWork, mapper, cacheService);

        //    //Act
        //    var actionResult = await controller.Get(id);

        //    //Assert
        //    var result = Assert.IsType<ObjectResult>(actionResult);
        //    Assert.IsAssignableFrom<Result<object>>(result.Value);
        //    Assert.Equal(500, result.StatusCode);
        //}
    }
}

