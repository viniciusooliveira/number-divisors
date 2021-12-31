using System;
using System.Collections.Generic;
using Domain.Models;

namespace Domain.Contracts.Business
{
    public interface INumberBusiness
    {
        IList<Factor> ListFactors(long n);

        IEnumerable<Factor> ListFactorsAsEnumerable(long n);

        IAsyncEnumerable<Factor> ListFactorsAsAsyncEnumerable(long n);

    }
}