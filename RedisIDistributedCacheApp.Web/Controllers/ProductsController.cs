using System.Data;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisIDistributedCacheApp.Web.Models;

namespace RedisIDistributedCacheApp.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IDistributedCache _distributedCache;
    
    public ProductsController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        
    }
    
    // Set
    public async Task<IActionResult> Index()
    {
        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
        options.AbsoluteExpiration=DateTimeOffset.Now.AddMinutes(1);

        Product product = new Product() {id = 1,name = "Kalem",price = (decimal)100.5,stock = 250};
        string jsonproduct = JsonConvert.SerializeObject(product);

        Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct);
         _distributedCache.Set("product:1",byteproduct);
      // await _distributedCache.SetStringAsync("product:1", jsonproduct, options);
        
        return View();
    }
    
    //Get
    
    public IActionResult Show()
    {
      
       var cache=  _distributedCache.Get("product:1");
      // Product product = JsonConvert.DeserializeObject<Product>(cache);
       ViewBag.cache = cache;
        return View();
    }
}