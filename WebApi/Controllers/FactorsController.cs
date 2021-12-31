using System.Collections.Generic;
using Domain.Contracts.Business;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FactorsController
    {
        private readonly ILogger<FactorsController> _logger;
        private readonly INumberBusiness _numberBusiness;

        public FactorsController(ILogger<FactorsController> logger, INumberBusiness numberBusiness)
        {
            _logger = logger;
            _numberBusiness = numberBusiness;
        }

        /// <summary>
        /// Return a list containing all the number factors, telling which ones are prime numbers.
        /// </summary>
        /// <param name="number">The desired number.</param>
        /// <returns>A list of factors.</returns>
        [HttpGet]
        [Route("{number}")]
        public IAsyncEnumerable<Factor> Get([FromRoute]long number)
        {
            return _numberBusiness.ListFactorsAsAsyncEnumerable(number);
        }
    }
}