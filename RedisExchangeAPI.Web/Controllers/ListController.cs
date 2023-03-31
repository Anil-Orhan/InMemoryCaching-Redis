using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class ListController : Controller
{
    private readonly RedisService _redisService;
    private readonly IDatabase _database;
    private readonly string? _listKey = "namesList";

    public ListController(RedisService redisService)
    {
        _redisService = redisService;
        _database = _redisService.GetDb(0);
    }

    // GET
    public IActionResult Index()
    {
        var namesList = new List<string>();
        if (_database.KeyExists(_listKey))
            _database.ListRange(_listKey).ToList().ForEach(x => { namesList.Add(x.ToString()); });
        return View(namesList);
    }

    [HttpPost]
    public IActionResult Add(string name, string method)
    {
        _database.KeyExpire(_listKey, DateTime.Now.AddMinutes(5));
        if (method == "" || method == null)
            ListLeftPush(name);
        else if (method == "ListRightPush")
            ListRightPush(name);
        else if (method == "ListLeftPush") ListLeftPush(name);
        
        return RedirectToAction("Index");
    }


    //Listenin sonuna ekler
    [HttpPost]
    public IActionResult ListRightPush(string name)
    {
        _database.ListRightPush(_listKey, name);
        return RedirectToAction("Index");
    }

    //Listenin başına ekler
    [HttpPost]
    public IActionResult ListLeftPush(string name)
    {
        _database.ListLeftPush(_listKey, name);
        return RedirectToAction("Index");
    }

    // Verilen Value'yu siler
    public IActionResult RemoveItem(string name)
    {
        _database.ListRemoveAsync(_listKey, name).Wait();
        return RedirectToAction("Index");
    }

    // Listenin başından 1 index siler
    public IActionResult ListLeftPopAsync()
    {
        _database.ListLeftPopAsync(_listKey);
        return RedirectToAction("Index");
    }

    // Listenin sonundan 1 index siler
    public IActionResult ListRightPopAsync()
    {
        _database.ListRightPopAsync(_listKey);
        return RedirectToAction("Index");
    }
}
