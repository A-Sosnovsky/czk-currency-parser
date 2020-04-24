using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parser.DAL;
using Parser.DAL.Context;
using Parser.Services.Parsing;

namespace Parser.Services
{
    internal class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyProvider _currencyProvider;
        private readonly IRepository _repository;

        public CurrencyService(ICurrencyProvider currencyProvider, IRepository repository)
        {
            _currencyProvider = currencyProvider;
            _repository = repository;
        }

        public async Task ParseByDate(DateTime date)
        {
            var result = await _currencyProvider.GetByDate(date);

            var parser = new Parser<DayValueMapping>();
            var parseResult = parser.Parse(result);
            var dbCurrencies = await _repository.Query<Currency>()
                .ToDictionaryAsync(c => c.Name, StringComparer.OrdinalIgnoreCase);

            var insert = parseResult.Select(r => new DAL.Context.CurrencyValue
            {
                Date = DateTime.Now,
                Value = r.Value,
                Currency = dbCurrencies.TryGetValue(r.Name, out var currency)
                    ? currency
                    : new Currency {Name = r.Name}
            });

            await _repository.BulkInsertAsync(insert);
            _repository.CommitTransaction();
        }

        public async Task ParseByYear(int year)
        {
            if (year <= 0 || year > DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid year value", nameof(year));
            }
            
            var result = await _currencyProvider.GetByYear(year);
        }
    }
}