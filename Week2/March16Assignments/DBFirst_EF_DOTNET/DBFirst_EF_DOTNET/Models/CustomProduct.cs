using System;
using System.Collections.Generic;

namespace DBFirst_EF_DOTNET.Models;

public partial class CustomProduct
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public int Stock { get; set; }
}
