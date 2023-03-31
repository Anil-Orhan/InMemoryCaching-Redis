using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers;

public class SortedSetController : Controller
{
    
    private readonly RedisService _redisService;
    private readonly IDatabase _database;
    private string? _listKey = "namesSortedSet";
    Order Order = Order.Descending;
    sbyte Start = 0;
    sbyte Stop =100;
    sbyte GetType =0;

    public SortedSetController(RedisService redisService)
    {
        _redisService = redisService;
        _database = _redisService.GetDb(0);
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var list = await SelectOrder();
        
        
        return View(list);
    }

    public async Task<HashSet<string>> SelectOrder()
    {
      
        // GetTypes
        // 0-SortedSetRangeByRankAsync
        // 1-SortedSetRangeByValueAsync
        // 2-SortedSetScan
        
        
        HashSet<string> list = new HashSet<string>();
        if (_database.KeyExists(_listKey))
        {
            if (GetType==0)
            {
                _database.SortedSetRangeByRankAsync(_listKey,Start,Stop,Order).Result.ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }
            else if (GetType==1)
            {
                _database.SortedSetRangeByValueAsync(_listKey).Result.ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }
            else if (GetType==2)
            {
                _database.SortedSetScan(_listKey).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }
           
        }

        return list;
    }

    [HttpPost]
    public IActionResult Add(string name, int score)
    {
        _database.KeyExpire(_listKey,DateTime.Now.AddMinutes(1));
        _database.SortedSetAdd(_listKey, name, score);
        
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Remove(string name)
    {
        if (GetType==2)
        {
            _database.KeyExpire(_listKey,DateTime.Now.AddMinutes(1));
            string[] split = name.Split(":");
            await _database.SortedSetRemoveAsync(_listKey, split[0]);
            
            return RedirectToAction("Index");
        }
        _database.KeyExpire(_listKey,DateTime.Now.AddMinutes(1));
       await _database.SortedSetRemoveAsync(_listKey, name);
        
        return RedirectToAction("Index");
    }
    
}