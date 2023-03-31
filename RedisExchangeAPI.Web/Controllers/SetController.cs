using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class SetController : Controller
{
    
    private readonly RedisService _redisService;
    private readonly IDatabase _database;
    private string? _listKey = "namesSet";
    public SetController(RedisService redisService)
    {
        _redisService = redisService;
        _database = _redisService.GetDb(0);
    }
    // GET
    public IActionResult Index()
    {
        HashSet<string> namesList = new HashSet<string>();
        if (_database.KeyExists(_listKey))
        {
            _database.SetMembers(_listKey).ToList().ForEach(x =>
            {
                namesList.Add(x.ToString());
            });
        }

        return View(namesList);
    }

    [HttpPost]
    public IActionResult Add(string name)
    {
        _database.KeyExpire(_listKey, DateTime.Now.AddMinutes(5));
        _database.SetAdd(_listKey, name);
        return RedirectToAction("Index");
    }
    
    
    public IActionResult Remove(string name)
    {
        _database.KeyExpire(_listKey, DateTime.Now.AddMinutes(5));
        _database.SetRemove(_listKey, name);
        return RedirectToAction("Index");
    }
}