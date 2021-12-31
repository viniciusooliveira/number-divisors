using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts.Infrastructure;
using StackExchange.Redis;

namespace Infrastructure
{
    public class RedisBaseRepository : IRedisRepository
    {
        private readonly ConnectionMultiplexer _con;

        public RedisBaseRepository(string connection, int expiration = 60)
        {
            _con = ConnectionMultiplexer.Connect(connection);
            
        }

        public async Task<bool> KeyExists(string key)
        {
            var db = _con.GetDatabase();
            return await db.KeyExistsAsync(key);
        }

        public async IAsyncEnumerable<T> SaveList<T>(string key, IAsyncEnumerable<T> list)
        {
            var db = _con.GetDatabase();
            await foreach (var item in list)
            {
                await db.ListRightPushAsync(key, JsonSerializer.SerializeToUtf8Bytes(item));
                await db.KeyExpireAsync(key, DateTime.Now.AddSeconds(60));
                yield return item;
            }
        }

        public async IAsyncEnumerable<T> GetList<T>(string key)
        {
            var db = _con.GetDatabase();

            var length = await db.ListLengthAsync(key);

            for (long i = 0; i < length; i++)
            {
                var item = await db.ListGetByIndexAsync(key, i);
                yield return JsonSerializer.Deserialize<T>(item);
            }
        }
    }
}