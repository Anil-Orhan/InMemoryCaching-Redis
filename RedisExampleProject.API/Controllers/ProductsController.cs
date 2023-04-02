
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExampleProject.API.Models;
using RedisExampleProject.API.Repository;
using RedisExampleProject.Cache;
using StackExchange.Redis;

namespace RedisExampleProject.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly RedisService _redisService;
    private readonly IDatabase _database;

    public ProductsController(IProductRepository productRepository, RedisService redisService, IDatabase database)
    {
        _productRepository = productRepository;
        _redisService = redisService;
        _database = database;
   
        _database.StringSet("ProductTest2", "Test2");
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        return Created(string.Empty,await _productRepository.CreateAsync(product));
        
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _productRepository.GetAsync());
    }
    
        
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _productRepository.GetByIdAsync(id));
    }

}
