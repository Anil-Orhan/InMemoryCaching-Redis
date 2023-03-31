using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class StringController : Controller
{
    private readonly RedisService _redisService;
    private readonly IDatabase _database;

        public StringController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(0);
            
        }
    // GET
    public IActionResult Index()
    {
        StringSet("StringControllerKey:1","Value1");
        return View();
    }

    // Set String Value
    public void StringSet(string key, string value)
    {
        _database.StringSet(key, value);
    }
    
    // Get String Value
    public string StringGet(string key)
    {
        return _database.StringGet(key).ToString();
    }
    
    // Value ++
    public string StringIncrement(string key)
    {
        return _database.StringIncrement(key).ToString();
    }
    
    // Value --
    public string StringDecrement(string key)
    {
        return _database.StringDecrement(key).ToString();
    }
    
    // Get Value Length
    public long StringLength(string key)
    {
        return _database.StringLength(key);
    }
    
    // Append on value
    public string StringAppend(string key,string value)
    {
        return _database.StringAppend(key,value).ToString();
    }
    
    // Delete
    public string StringGetDelete(string key)
    {
        return _database.StringGetDelete(key).ToString();
    }
    
    
}