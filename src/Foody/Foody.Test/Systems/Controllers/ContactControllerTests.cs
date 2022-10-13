using Foody.Test.Helpers;
using Foody.Test.Fixtures;
using Microsoft.AspNetCore.Mvc;


namespace Foody.Test.Systems.Controllers
{
    public class ContactControllerTests : TestBase
    {
        static readonly int id = 0;
        readonly string key = $"{id}";
        readonly string search = string.Empty;

        [Fact]
        public async void Get_All_ReturnOk()
        {
            //Arrange
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(string.Empty);

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public async void Get_All_InvokeUnitOfWork_ReturnOk()
        {
            //Arrange
            var fakeData = A.CollectionOfDummy<Contact>(5).AsEnumerable();
            A.CallTo(() => unitofWork.Contacts.All(search)).Returns(Task.FromResult(fakeData));
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(search);

            //Assert
            A.CallTo(() => unitofWork.Contacts.All(search)).MustHaveHappenedOnceExactly();
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(10)]
        public async void Get_ListOfInquiries_ReturnOk(int count)
        {
            //Arrange
            var fakeData = A.CollectionOfDummy<Contact>(count).AsEnumerable();
            A.CallTo(() => unitofWork.Contacts.All(search)).Returns(Task.FromResult(fakeData));
            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(search);

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<Pagination<Contact>>(result.Value);
            Assert.True(data.Success);
            Assert.NotNull(data.Content);
            Assert.Equal(count, data.Content.Count());
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
            var fakeData = A.CollectionOfDummy<Contact>(3).AsEnumerable();
            A.CallTo(() => cacheService.GetData<Contact>($"{id}")).Returns(Task.FromResult((Contact?)null));
            A.CallTo(() => unitofWork.Contacts.Get(id))
                .Returns(Task.FromResult(fakeData.FirstOrDefault(fk => fk.Id == id)));

            var controller = new ContactController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Get(id);

            //Assert
            A.CallTo(() => unitofWork.Contacts.Get(id)).MustHaveHappenedOnceExactly();
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Theory]
        [InlineData(213)]
        [InlineData(215)]
        [InlineData(217)]
        public async void Get_Inquiry_ReturnOk(int id)
        {
            //Arrange
            var fakeData = InquiriesFixtures.GetContacts().FirstOrDefault(c => c.Id == id);
            A.CallTo(() => cacheService.GetData<Contact>($"{id}")).Returns(Task.FromResult((Contact?)null));
            A.CallTo(() => unitofWork.Contacts.Get(id))
                .Returns(Task.FromResult(fakeData));
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
        public async Task Get_Inquiry_ReturnNotFound()
        {
            //Arrange
            int id = 300;
            A.CallTo(() => cacheService.GetData<Contact>($"{id}")).Returns(Task.FromResult((Contact?)null));
            A.CallTo(() => unitofWork.Contacts.Get(id)).Returns(Task.FromResult((Contact?)null));
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

        //[Fact]
        //public async Task Get_Inquiry_ReturnServerErrorr()
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

