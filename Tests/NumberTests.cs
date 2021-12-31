using System.Linq;
using Domain.Business;
using Domain.Contracts.Business;
using Tests.Mock;
using Xunit;

namespace Tests
{
    public class NumberTests
    {
        private readonly INumberBusiness _numberBusiness = new NumberBusiness(new RedisMockRepository());

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(5, true)]
        [InlineData(7, true)]
        [InlineData(11, true)]
        [InlineData(4, false)]
        [InlineData(6, false)]
        [InlineData(8, false)]
        [InlineData(9, false)]
        [InlineData(10, false)]
        [InlineData(int.MaxValue, true)]
        public void TestPrime(int n, bool res)
        {
            Assert.Equal(res, _numberBusiness.IsPrime(n));
        }

        [Fact]
        public void TestListFactors()
        {
            var expectedResults = new long[]{ 1, 3, 5, 9, 15, 45};
            var expectedPrimes = new long[] { 1, 3, 5};
            var res = _numberBusiness.ListFactorsAsEnumerable(45).ToList();
            
            Assert.Equal(6, res.Count);
            Assert.Equal(expectedResults, res.Select(x => x.Number).ToArray());
            Assert.Equal(expectedPrimes, res.Where(x => x.IsPrime)
                .Select(x => x.Number).ToArray());
        }
    }
}