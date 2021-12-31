using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Contracts.Infrastructure
{
    public interface IRedisRepository
    {
        Task<bool> KeyExists(string key);
        IAsyncEnumerable<T> SaveList<T>(string key, IAsyncEnumerable<T> list);
        IAsyncEnumerable<T> GetList<T>(string key);
    }
}