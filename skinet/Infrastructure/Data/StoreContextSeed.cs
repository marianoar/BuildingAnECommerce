using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync (StoreContext context)
        {
            if(!context.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products == null)
                {
                    return;
                }
                foreach (var product in products)
                {
                    context.Products.Add(product);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
