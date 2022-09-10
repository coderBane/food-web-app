using System;
using Foody.Entities.Models;

namespace Foody.Data.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    bool Exists(int id);

    Task Update(Product product);
}

