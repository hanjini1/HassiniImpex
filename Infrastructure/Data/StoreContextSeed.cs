using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entites;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
  public class StoreContextSeed
  {
    public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
    {
      try
      {
        if (!context.ProductBrands.Any())
        {
          var brandData = File.ReadAllBytes("../Infrastructure/Data/SeedData/brands.json");
          var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
          if (brands != null)
          {
            context.ProductBrands.AddRange(brands);
            await context.SaveChangesAsync();
          }
        }
        if (!context.ProductTypes.Any())
        {
          var typesData = File.ReadAllBytes("../Infrastructure/Data/SeedData/types.json");
          var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
          if (types != null)
          {
            context.ProductTypes.AddRange(types);
            await context.SaveChangesAsync();
          }
        }
        if (!context.Products.Any())
        {
          var productsData = File.ReadAllBytes("../Infrastructure/Data/SeedData/products.json");
          var products = JsonSerializer.Deserialize<List<Product>>(productsData);
          if (products != null)
          {
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
          }
        }
      }
      catch (Exception ex)
      {

        var logger = loggerFactory.CreateLogger<StoreContextSeed>();
        logger.LogError(ex.Message);
      }
    }
  }
}