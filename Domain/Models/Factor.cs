namespace Domain.Models
{
    public record Factor
    {
        /// <summary>
        /// The actual factor.
        /// </summary>
        public long Number { get; init; } = default;
        
        /// <summary>
        /// Indicates if the number is prime or not.
        /// </summary>
        public bool IsPrime { get; init; } = default;

        public Factor(long number, bool isPrime)
        {
            Number = number;
            IsPrime = isPrime;
        }
    }
}