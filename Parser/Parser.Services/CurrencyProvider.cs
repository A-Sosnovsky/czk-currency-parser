using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Parser.Services
{
    internal class CurrencyProvider : ICurrencyProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string DailyUrlTemplate = "daily.txt?date={0}";
        private const string YearUrlTemplate = "year.txt?year={0}";

        public CurrencyProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetByYear(int year)
        {
            if (year < 0)
            {
                throw new ArgumentException("Value can't be less than 0", nameof(year));
            }

            if (year > DateTime.Now.Year)
            {
                throw new ArgumentException("Value can't be more than this year", nameof(year));
            }

            return await GetAsync(string.Format(YearUrlTemplate, year));
        }

        public async Task<string> GetByDate(DateTime date)
        {
            var dateArg = date.ToString("dd.mm.yyyy");
            return await GetAsync(string.Format(DailyUrlTemplate, dateArg));
        }

        private async Task<string> GetAsync(string uri)
        {
            using var client = _httpClientFactory.CreateClient("CurrencyClient");
            try
            {
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}