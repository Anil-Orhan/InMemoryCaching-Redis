using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisIDistributedCacheApp.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IDistributedCache _distributedCache;
    
    public ProductsController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _distributedCache.
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
}