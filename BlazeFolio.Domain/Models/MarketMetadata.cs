namespace BlazeFolio.Domain.Models
{
    public class MarketMetadata
    {
        /// <summary>
        /// The full company or security name
        /// </summary>
        public string? LongName { get; set; }

        /// <summary>
        /// The type of security (e.g., EQUITY, ETF, INDEX)
        /// </summary>
        public string? QuoteType { get; set; }
    }
}
