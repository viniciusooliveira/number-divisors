using System;
using System.Collections.Generic;
using Domain.Models;

namespace Domain.Contracts.Business
{
    public interface INumberBusiness
    {

        IEnumerable<Factor> ListFactorsAsEnumerable(long n);

        IAsyncEnumerable<Factor> ListFactorsAsAsyncEnumerable(long n);

        bool IsPrime(long n);

    }
}