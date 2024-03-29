﻿namespace Foody.Entities.Models;

public class Category : Item
{
    public ICollection<Product> Products { get; set; }

    public Category() => Products = new HashSet<Product>();
}
