using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisExchange.Models;
using RedisExchange.Services;
using StackExchange.Redis;

namespace RedisExchange.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        public List<Product> products { get; set; }
        private RedisService _redisService;
        private IDatabase _db;
        private string listKey = "_popularProducts";
        public ProductsController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDB(3);

            products = ProductsList.GenerateProductsList();

            if (_db.ListGetByIndex(listKey, 0).IsNullOrEmpty)
            {
                products.Where(x => x.IsPopular == true).ToList().ForEach(product =>
                {
                    var data = JsonConvert.SerializeObject(product);
                    _db.ListLeftPush(listKey, data);
                });
            }
        }
        [HttpGet]
        public List<Product> AlllProducts()
        {
            return products;
        }
        [HttpGet("Rpopular")]
        public List<Product> PopularProductsWithRedis()
        {
            List<Product> products = new List<Product>();

            _db.ListRange(listKey).ToList().ForEach(product =>
            {
                Product data = JsonConvert.DeserializeObject<Product>(product);
                products.Add(data);
            });

            return products;
        }

        [HttpGet("popular")]
        public List<Product> PopularProducts()
        {
            List<Product> popularProducts = new List<Product>();

            products.Where(x => x.IsPopular == true).ToList().ForEach(product =>
            {
                popularProducts.Add(product);
            });

            return popularProducts;
        }
    }
}
