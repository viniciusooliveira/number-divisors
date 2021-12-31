using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Contracts.Infrastructure;

namespace Tests.Mock
{
    public class RedisMockRepository : IRedisRepository
    {
        public Task<bool> KeyExists(string key)
        {
            return Task.FromResult(false);
        }

        public async IAsyncEnumerable<T> SaveList<T>(string key, IAsyncEnumerable<T> list)
        {
            await foreach (var item in list)
                yield return item;
        }

        public async IAsyncEnumerable<T> GetList<T>(string key)
        {
            yield break;
        }
    }
}