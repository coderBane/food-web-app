using Foody.Data.Data;
using Foody.Data.Interfaces;
using Foody.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Foody.Data.Repositories;

public sealed class CategoryRepository : ItemRepository<Category>, ICategoryRepository
{
    public CategoryRepository(FoodyDbContext context) : base(context) { }
}
