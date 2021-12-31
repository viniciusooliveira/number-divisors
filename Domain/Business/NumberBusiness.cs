
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Contracts.Business;
using Domain.Contracts.Infrastructure;
using Domain.Models;

namespace Domain.Business
{
    public class NumberBusiness : INumberBusiness
    {
        private readonly IRedisRepository _redis;
        private readonly string _keyPrefix = "numbers:factors";
        
        public NumberBusiness(IRedisRepository redis)
        {
            _redis = redis;
        }
        
        public IList<Factor> ListFactors (long n)
        {
            var factors = new List<Factor>
            {
                new(1, true), 
                new(n, IsPrime(n))
            };

            for (long i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i != 0) continue;
                factors.Add(new Factor(i, IsPrime(i)));
                factors.Add(new Factor(n/i, IsPrime(n/i)));
            }

            return factors;
        }
        
        public IEnumerable<Factor> ListFactorsAsEnumerable(long n)
        {
            yield return new Factor(1, true);

            if (n <= 1) yield break;

            var lastDivisors = new List<long>{n};

            for (long i = 2; i*i <= n; i++)
            {
                if (n % i != 0) continue;
                
                yield return new Factor(i, IsPrime(i));
                if(n/i != i)
                    lastDivisors.Add(n/i);
            }

            for (var i = lastDivisors.Count-1; i >= 0; i--)
            {
                yield return new Factor(lastDivisors[i], IsPrime(lastDivisors[i]));
            }
        }

        // public IAsyncEnumerable<Factor> ListFactorsAsAsyncEnumerable(long n) =>
        //     ListFactorsAsEnumerable(n).ToAsyncEnumerable();
        
        public async IAsyncEnumerable<Factor> ListFactorsAsAsyncEnumerable(long n)
        {
            IAsyncEnumerable<Factor> enumerable;

            if (await _redis.KeyExists(GenerateKey(n)))
            {
                enumerable = _redis.GetList<Factor>(GenerateKey(n));
            }
            else
            {
                enumerable = _redis.SaveList(GenerateKey(n), ListFactorsAsEnumerable(n).ToAsyncEnumerable());
            }

            await foreach (var item in enumerable)
            {
                yield return item;
            }
        }

        private bool IsPrime(long n)
        {
            if (n < 1) return false;
            if (n <= 2) return true;
            if (n % 2 == 0) return false;

            for (long i = 3; i*i <= n; i += 2)
                if (n % i == 0)
                    return false;
            return true;
        }

        private string GenerateKey(long n)
            => $"{_keyPrefix}:{n}";
    }
}