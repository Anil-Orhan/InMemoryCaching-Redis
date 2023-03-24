using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace RedisInMemory.Web.Controllers;

public class ProductController : Controller
{
    private IMemoryCache _memoryCache;
    public ProductController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    // GET
    public IActionResult Index()
    {
        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
        options.AbsoluteExpiration=DateTime.Now.AddSeconds(20);
        
       // options.Priority = CacheItemPriority.Low; //Düşük Öncelikli Cache
       // options.Priority = CacheItemPriority.Normal; //Normal Öncelikli Cache
        options.Priority = CacheItemPriority.High;//Yüksek Öncelikli Cache
       // options.Priority = CacheItemPriority.NeverRemove; //Ram'den asla silinmeyecek Cache
        
        _memoryCache.Set<string>("time",DateTime.Now.ToString(),options);
        
        return View();
    }

    public IActionResult Show()
    {
        _memoryCache.TryGetValue("time", out string timecache);
       
       
       ViewBag.time= timecache;
       return View();
    }
}