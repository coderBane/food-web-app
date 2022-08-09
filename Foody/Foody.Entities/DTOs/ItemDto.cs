using System;

#nullable disable

namespace Foody.Entities.DTOs;

public class ItemDto
{
    public string Name { get; set; }

    public bool IsActive { get; set; }
}

public class ItemDetailDto
{
   
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public string ImageUri { get; set; }

    public DateTime AddedOn { get; set; }

    public DateTime Updated { get; set; }
}

