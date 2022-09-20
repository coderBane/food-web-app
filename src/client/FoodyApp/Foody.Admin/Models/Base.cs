using System;

namespace Foody.Admin.Models;

public abstract class Base
{
    private readonly JsonSerializerOptions _options;

    public Base()
    {
        _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, _options);
    }
}

