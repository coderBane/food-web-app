using Foody.Entities.DTOs;
using Foody.Entities.Models;
using Foody.Utilities.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Foody.Test.Controllers
{
    public class CategoryControllerTests : TestBase
    {
        [Fact]
        public async Task GetAllCategoriesAsync()
        {
            //Arrange
            int count = 6;
            var fakeData = GetCategories(6);
            A.CallTo(() => unitofWork.Categories.All(string.Empty)).Returns(Task.FromResult(fakeData));
            var controller = new CategoryController(unitofWork, mapper);

            //Act
            var actionResult = await controller.Get();

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var data = Assert.IsAssignableFrom<Pagination<CategoryDto>>(result.Value);
            Assert.True(data.Success);
            Assert.NotNull(data.Content);
            Assert.Equal(count, data.Content.Count());
        }

        [Fact]
        public async Task Get_Category_ReturnOk()
        {
            //Arrange
            int id = 311;
            var collection = GetCategories(3);
            collection.First().Id = id;
            A.CallTo(() => unitofWork.Categories.Get(id)).
                Returns(Task.FromResult(collection.FirstOrDefault(c => c.Id == id)));
            var controller = new CategoryController(unitofWork, mapper);

            //Act
            var actionresult = await controller.Get(id);

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionresult);
            var data = Assert.IsAssignableFrom<Result<CategoryDetailDto>>(result.Value);
            Assert.True(data.Success);
            Assert.NotNull(data.Content);
            Assert.Equal(id, data.Content.Id);
        }

        [Fact]
        public async Task Get_Category_ReturnNotFound()
        {
            //Arrange
            int id = 3;
            A.CallTo(() => unitofWork.Categories.Get(id)).Returns(Task.FromResult((Category?)null));
            var controller = new CategoryController(unitofWork, mapper);

            //Act
            var actionresult = await controller.Get(id);

            //Assert
            var result = Assert.IsType<NotFoundObjectResult>(actionresult);
            var data = Assert.IsAssignableFrom<Result<CategoryDetailDto>>(result.Value);
            Assert.False(data.Success);
            Assert.Null(data.Content);
            Assert.NotNull(data.Error);
        }

        internal static IEnumerable<Category> GetCategories(int count) =>
            A.CollectionOfDummy<Category>(count).AsEnumerable();
    }
}

