using System;
using System.Collections.Generic;

namespace DBFirst_EF_DOTNET.Models;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
