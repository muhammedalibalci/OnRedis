using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchange.Models
{
    public static class ProductsList
    {
        public static List<Product> GenerateProductsList()
        {
            List<Product> products = new List<Product>();

            for (int i = 0; i < 100000; i++)
            {
                Product product = new Product();
                product.Id = i;
                product.Name = "PC" + i;
                product.Price = 5000 + i;
                if (i % 10 == 0)
                {
                    product.IsPopular = true;
                }
                products.Add(product);
            };

            return products;
        }
    }
}
