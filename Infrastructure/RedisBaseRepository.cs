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
        private ConnectionMultiplexer _con;
        private readonly string _conStr;
        private readonly int _keyExpiration;

        public RedisBaseRepository(string connection, int expiration = 60)
        {
            _conStr = connection;
            _keyExpiration = expiration;
            TryConnect();
        }

        private void TryConnect()
        {
            if (_con is { IsConnected: true }) return;
            try
            {
                _con = ConnectionMultiplexer.Connect(_conStr);
            }
            catch
            {
                // ignored
            }
        }

        private IDatabase GetDatabase()
        {
            try
            {
                TryConnect();
                return _con.GetDatabase();
            }
            catch
            {
                // ignored
            }

            return null;
        }
        
        
        public async Task<bool> KeyExists(string key)
        {
            var db = GetDatabase();
            
            if (db != null)
            {
                return await db?.KeyExistsAsync(key);
            }
            return false;
        }

        public async IAsyncEnumerable<T> SaveList<T>(string key, IAsyncEnumerable<T> list)
        {
            var db = GetDatabase();
            await foreach (var item in list)
            {
                if (db != null)
                {
                    await db.ListRightPushAsync(key, JsonSerializer.SerializeToUtf8Bytes(item));
                    await db.KeyExpireAsync(key, DateTime.Now.AddSeconds(_keyExpiration));
                }
                yield return item;
            }
        }

        public async IAsyncEnumerable<T> GetList<T>(string key)
        {
            var db = GetDatabase();

            if (db == null) yield break;
            
            var length = await db.ListLengthAsync(key);

            for (long i = 0; i < length; i++)
            {
                var item = await db.ListGetByIndexAsync(key, i);
                yield return JsonSerializer.Deserialize<T>(item);
            }
        }
    }
}