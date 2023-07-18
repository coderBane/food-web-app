using Foody.Data.Data;
using System.Runtime.InteropServices;


namespace Foody.Data.Repositories;

public sealed class ProductRepository : ItemRepository<Product>, IProductRepository
{
    public ProductRepository(FoodyDbContext context, ILogger logger) : base(context, logger) { }

    public async Task<IReadOnlyDictionary<string, List<ProdCategoryDto>>?> ProducstByCategory()
    {
        try
        {
            return await _dbSet.Where(p => p.State == 1)
                               .Include(p => p.Image)
                               .Include(p => p.Category)
                               .GroupBy(p => p.Category.Name, (k, p) => new
                               {
                                   Category = k,
                                   Products = p.Select(p => new ProdCategoryDto
                                   {
                                       Id = p.Id,
                                       Name = p.Name,
                                       Price = p.Price,
                                       IsActive = p.IsActive,
                                       Category = p.CategoryId,
                                       Image = Convert.ToBase64String(p.Image.Content),
                                       Description = p.Description
                                   }).ToList()
                               }).ToDictionaryAsync(p => p.Category, p => p.Products);
        }
        catch (Exception)
        {

        }

        return null;
    }

    public override async Task<IEnumerable<Product>> AllAsync([Optional] string search)
    {
        try
        {
            var products = _dbSet.Where(p => p.State == 1)
                                 .Include(p => p.Category)
                                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string filter = search.ToLower();
                products = products.Where(p => p.Name.ToLower().Contains(filter)
                                            || p.Category.Name.ToLower().Contains(filter));
            }

            return await products.AsNoTracking().ToListAsync();
        }
        catch (Exception)
        {

        }

        return Enumerable.Empty<Product>();
    }

    public override async Task<Product?> GetAsync(int id)
    {
        try
        {
            return await _dbSet.Where(p => p.State == 1)
                               .Include(p => p.Image)
                               .Include(p => p.Category)
                               .AsNoTracking()
                               .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception)
        {

        }

        return null;
    }

    public override Task UpdateAsync(Product product)
    {
        var existing = _dbSet.Find(product.Id);

        if (existing is not null)
        {
            existing.Name = product.Name;
            existing.IsActive = product.IsActive;
            existing.Price = product.Price;
            existing.Quantity = product.Quantity;
            existing.CategoryId = product.CategoryId;
            existing.Description = product.Description;

            if (product.Image is not null)
            {
                existing.Image = product.Image;
                existing.ImageUri = product.ImageUri;
            }

            existing.Updated = DateTime.UtcNow;
        }

        return Task.CompletedTask;
    }
}

