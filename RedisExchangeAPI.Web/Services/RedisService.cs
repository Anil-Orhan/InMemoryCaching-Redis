using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services;

public class RedisService
{
   
    
    public IDatabase db { get; set; }
    private  ConnectionMultiplexer _redis;
    
    public RedisService(string Url)
    {
        _redis=ConnectionMultiplexer.Connect(Url);
        
       
    }

   

    public IDatabase GetDb(int db)
    {
        return _redis.GetDatabase(db);
    }
    
}