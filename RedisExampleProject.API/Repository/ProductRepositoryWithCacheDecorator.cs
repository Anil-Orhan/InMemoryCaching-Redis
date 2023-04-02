using System.Text.Json;
using RedisExampleProject.API.Models;
using RedisExampleProject.Cache;
using StackExchange.Redis;

namespace RedisExampleProject.API.Repository;

public class ProductRepositoryWithCacheDecorator:IProductRepository
{   
    private const string productKey="productCaches";
    private readonly IProductRepository _productRepository;
    private readonly RedisService _redisService;
    private readonly IDatabase _database;

    public ProductRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService, IDatabase database)
    {
        _productRepository = productRepository;
        _redisService = redisService;
        _database = database;
    }

    public async Task<List<Product?>> GetAsync()
    {
        if (!await _database.KeyExistsAsync(productKey))
        {
           return  await LoadToCacheFromDbAsync();
        }

        var products = new List<Product>();
        var cacheProducts = await _database.HashGetAllAsync(productKey);
        foreach (var item in  cacheProducts.ToList())
        {
            var product = JsonSerializer.Deserialize<Product>(item.Value);
            products.Add(product);
        }

        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        if (await _database.KeyExistsAsync(productKey))
        {
            var product = await _database.HashGetAsync(productKey, id);
            return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
        }

        var products = await LoadToCacheFromDbAsync();
        return products.FirstOrDefault(x=>x.Id==id);

    }

    public async Task<Product?> CreateAsync(Product? product)
    {
        var newProduct = await _productRepository.CreateAsync(product);

        if (await _database.KeyExistsAsync(productKey))
        {
            await _database.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProduct));
        }

        return newProduct;
    }

    //Db'den datayı cacheler
    private async Task<List<Product>> LoadToCacheFromDbAsync()
    {
        var products = await _productRepository.GetAsync();
        products.ForEach(x =>
        {
            _database.HashSetAsync(productKey, x.Id, JsonSerializer.Serialize(x));
        });
        return products;
    }
    
}
