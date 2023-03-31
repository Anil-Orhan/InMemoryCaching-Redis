using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class HashController : Controller
{
    private readonly IDatabase _database;
    private readonly RedisService _redisService;
    private readonly string? _listKey = "namesHash";

    public HashController(RedisService redisService)
    {
        _redisService = redisService;
        _database = _redisService.GetDb(0);
    }

    // GET
    public IActionResult Index()
    {
        var list = new Dictionary<string, string>();
        if (_database.KeyExists(_listKey))
            _database.HashGetAll(_listKey).ToList().ForEach(x => { list.Add(x.Name, x.Value); });
        return View(list);
    }

    
    // Yeni çift ekler
    [HttpPost]
    public IActionResult Add(string key, string value)
    {
        _database.HashSet(_listKey, key, value);
        return RedirectToAction("Index");
    }

    // Çifti siler
    public IActionResult HashDelete(string key, string value)
    {
        _database.HashDelete(_listKey, key);

        return RedirectToAction("Index");
    }

    // Tek bir value çeker
    public IActionResult HashGet(string key, string value)
    {
        _database.HashGet(_listKey, key);
        return RedirectToAction("Index");
    }

    // Tüm listeyi çeker
    public IActionResult HashGetAll()
    {
        _database.HashGetAll(_listKey);
        return RedirectToAction("Index");
    }
}
