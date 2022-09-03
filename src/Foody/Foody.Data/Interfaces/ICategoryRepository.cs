using System;
using Foody.Entities.DTOs;
using Foody.Entities.Models;

namespace Foody.Data.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    bool Exists(int id);

    Task Update(Category category);
}

