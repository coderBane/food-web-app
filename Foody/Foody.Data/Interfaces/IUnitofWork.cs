using System;

namespace Foody.Data.Interfaces;

public interface IUnitofWork
{
    ICategoryRepository Categories { get; }

    IProductRepository Products { get; }

    Task CompleteAsync();
}

