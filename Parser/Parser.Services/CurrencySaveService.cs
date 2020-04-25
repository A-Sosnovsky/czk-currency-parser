using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parser.DAL;
using Parser.DAL.Context;
using Parser.Services.Parsing;

namespace Parser.Services
{
    internal class CurrencySaveService : ICurrencySaveService
    {
        private readonly ICurrencyProvider _currencyProvider;
        private readonly IRepository _repository;
        private readonly IParser<CurrencyValue> _dayParser = new Parser<CurrencyValue>(new DayParsingStrategy());
        private readonly IParser<CurrencyValue> _yearParser = new Parser<CurrencyValue>(new YearParsingStrategy());

        public CurrencySaveService(ICurrencyProvider currencyProvider, IRepository repository)
        {
            _currencyProvider = currencyProvider;
            _repository = repository;
        }

        public async Task SaveByDate(DateTime date)
        {
            var result = await _currencyProvider.GetByDate(date);
            var parseResult = _dayParser.Parse(result);
            await SaveToDb(parseResult);
        }

        public async Task SaveByYear(int year)
        {
            if (year <= 0 || year > DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid year value", nameof(year));
            }

            var result = await _currencyProvider.GetByYear(year);
            var parseResult = _yearParser.Parse(result);
            await SaveToDb(parseResult);
        }

        private async Task SaveToDb(IEnumerable<CurrencyValue> values)
        {
            var dbCurrencies = await _repository.Query<Currency>()
                .ToDictionaryAsync(c => c.Name, StringComparer.OrdinalIgnoreCase);

            var insert = values.Select(r => new DAL.Context.CurrencyValue
            {
                Date = r.Date,
                Value = r.Value / r.Amount,
                Currency = dbCurrencies.TryGetValue(r.Name, out var currency)
                    ? currency
                    : new Currency { Name = r.Name }
            });

            foreach (var value in insert)
            {
                await _repository.InsertAsync(value);
            }
            
            _repository.CommitTransaction();
        }
    }
}