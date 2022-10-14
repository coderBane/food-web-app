using Foody.Test.Helpers;
using Foody.Test.Fixtures;
using Foody.Utilities.Messages;


namespace Foody.Test.Systems.Controllers
{
    public class CategoryControllerTests : TestBase
    {
        private readonly string name = "Rice";

        [Fact]
        public async Task Post_Return201()
        {
            //Arrange
            var fakeData = A.Dummy<CategoryModDto>();
            fakeData.Name = name;

            var controller = new CategoryController(unitofWork, mapper, cacheService);
            controller.ObjectValidator = new ObjectValidator();

            //Act
            var actionResult = await controller.Post(fakeData);

            //Assert
            var result = Assert.IsType<CreatedAtActionResult>(actionResult);
            Assert.Null(result.ControllerName);
            Assert.Equal("Get", result.ActionName);
            Assert.Equal(0, result.RouteValues!.GetValueOrDefault("id"));
        }

        [Fact]
        public async Task Post_InvokeUnitOfWork_Return201()
        {
            //Arrange
            var fakeDto = A.Dummy<CategoryModDto>();
            fakeDto.Name = name;

            //var fakeEntity = mapper.Map<Category>(fakeDto);
            //A.CallTo(() => unitofWork.Categories.Add(fakeEntity)).Returns(Task.CompletedTask);

            var controller = new CategoryController(unitofWork, mapper, cacheService);
            controller.ObjectValidator = new ObjectValidator();

            //Act
            var actionResult = await controller.Post(fakeDto);

            //Assert
            A.CallTo(() => unitofWork.CompleteAsync()).MustHaveHappenedOnceExactly();
            Assert.IsType<CreatedAtActionResult>(actionResult);
        }

        [Fact]
        public async Task Post_WhenModelStateIsInvalid_Return422()
        {
            //Arrange
            var fakeData = A.Dummy<CategoryModDto>();
            fakeData.Name = "T"; // name less than 4 characters
            var controller = new CategoryController(unitofWork, mapper, cacheService);
            controller.ObjectValidator = new ObjectValidator();

            //Act
            var actionResult = await controller.Post(fakeData);

            //Assert
            var result = Assert.IsType<UnprocessableEntityObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<Result<CategoryDto>>(result.Value);
            Assert.False(data.Success);
            Assert.NotNull(data.Error);
        }

        [Fact]
        public async Task Post_NameIndexConstraintFailed_Returns409()
        {
            //Arrange
            var fakeData = A.Dummy<CategoryModDto>();
            fakeData.Name = name;

            A.CallTo(() => unitofWork.Categories.Exists(fakeData.Name)).Returns(Task.FromResult(true));

            var controller = new CategoryController(unitofWork, mapper, cacheService);
            controller.ObjectValidator = new ObjectValidator();
            
            //Act
            var actionResult = await controller.Post(fakeData);

            //Assert
            A.CallTo(() => unitofWork.CompleteAsync()).MustNotHaveHappened();
            var result = Assert.IsType<ConflictObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<Result<CategoryDto>>(result.Value);
            Assert.NotNull(data.Error);
            Assert.Equal(409, data.Error.Code);
            Assert.Null(data.Content);
        }

        [Fact]
        public async Task Put_Product_ReturnNoContent()
        {
            //Arrange
            var fakeDto = A.Dummy<CategoryModDto>();
            fakeDto.Name = name;
            fakeDto.IsActive = true;

            var fakeEntity = ItemFixtures.GetCategory();
            A.CallTo(() => unitofWork.Categories.Get(fakeEntity.Id)).Returns(Task.FromResult(fakeEntity));
            A.CallTo(() => unitofWork.Categories.Update(fakeEntity)).Returns(Task.CompletedTask);

            var controller = new CategoryController(unitofWork, mapper, cacheService);
            controller.ObjectValidator = new ObjectValidator();

            //Act
            var actionResult = await controller.Put(fakeEntity.Id, fakeDto);

            //Assert
            A.CallTo(() => unitofWork.CompleteAsync()).MustHaveHappenedOnceExactly();
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        public async Task Put_Product_ReturnNotFound()
        {
            //Arrange
            int id = 300;
            var fakeDto = A.Dummy<CategoryModDto>();

            A.CallTo(() => unitofWork.Categories.Get(id)).Returns(Task.FromResult((Category?)null));

            var controller = new CategoryController(unitofWork, mapper, cacheService);

            //Act
            var actionResult = await controller.Put(300, fakeDto);

            //Assert
            var result = Assert.IsType<NotFoundObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<Result<object>>(result.Value);
            Assert.False(data.Success);
            Assert.NotNull(data.Error);
            Assert.Equal(ErrorsMessage.Category.NotExist, data.Error.Message);
        }

        [Fact]
        public async Task Put_Product_WhenModelStateIsInvalid_Returns422()
        {
            //Arrange
            var fakeDto = A.Dummy<CategoryModDto>();
            fakeDto.ImageUpload = ItemFixtures.InvalidFileType();

            var fakeEntity = ItemFixtures.GetCategory();
            A.CallTo(() => unitofWork.Categories.Get(fakeEntity.Id)).Returns(Task.FromResult(fakeEntity));

            var controller = new CategoryController(unitofWork, mapper, cacheService);
            controller.ObjectValidator = new ObjectValidator();

            //Act
            var actionResult = await controller.Put(fakeEntity.Id, fakeDto);

            //Assert
            var result = Assert.IsType<UnprocessableEntityObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<Result<object>>(result.Value);
            Assert.False(data.Success);
        }

        [Fact]
        public async Task Put_Product_ConcurrencyExceptionThrown()
        {
            //Arrange
            var fakeDto = A.Dummy<CategoryModDto>();
            fakeDto.Name = name;

            A.CallTo(() => unitofWork.Categories.Exists(default(int))).Returns(false);
            A.CallTo(() => unitofWork.CompleteAsync()).ThrowsAsync(() => new DbUpdateConcurrencyException());

            var controller = new CategoryController(unitofWork, mapper, cacheService);
            controller.ObjectValidator = new ObjectValidator();

            //Act
            var actionResult = await controller.Put(default, fakeDto);

            //Assert
            var result = Assert.IsType<NotFoundObjectResult>(actionResult);
            var data = Assert.IsType<Result<object>>(result.Value);
        }
    }
}

