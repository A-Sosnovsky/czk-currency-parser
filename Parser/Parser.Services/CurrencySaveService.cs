using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CurrencySaveService> _logger;

        public CurrencySaveService(ICurrencyProvider currencyProvider, IRepository repository,
            ILogger<CurrencySaveService> logger)
        {
            _currencyProvider = currencyProvider;
            _repository = repository;
            _logger = logger;
        }

        public async Task SaveByDate(DateTime date)
        {
            var result = await _currencyProvider.GetByDate(date);
            var parseResult = _dayParser.Parse(result).ToArray();

            var incorrectValue = parseResult.FirstOrDefault(r => r.Date.Date != date.Date);
            if (incorrectValue == null)
            {
                await SaveToDb(parseResult);
            }
            else
            {
                _logger.LogInformation(
                    $"SaveByDate did not saved data because of different dates of request date ({date}) and result({incorrectValue.Date}).");
            }
        }

        public async Task SaveByYear(int year)
        {
            if (year <= 0 || year > DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid year value", nameof(year));
            }

            var result = await _currencyProvider.GetByYear(year);
            var parseResult = _yearParser.Parse(result).ToArray();

            var incorrectValue = parseResult.FirstOrDefault(r => r.Date.Year != year);
            if (incorrectValue == null)
            {
                await SaveToDb(parseResult);
            }
            else
            {
                _logger.LogInformation(
                    $"SaveByYear result contains not suitable value ({incorrectValue.Date.Year}) for request ({year})");
            }
        }

        private async Task SaveToDb(params CurrencyValue[] values)
        {
            var dbCurrencies = await _repository.Query<Currency>()
                .ToDictionaryAsync(c => c.Name, StringComparer.OrdinalIgnoreCase);

            _repository.BeginTransaction();
            foreach (var value in values)
            {
                if (!dbCurrencies.TryGetValue(value.Name, out var currency))
                {
                    currency = new Currency { Name = value.Name };
                    await _repository.InsertAsync(currency);
                    await _repository.SaveChangesAsync();
                    dbCurrencies.Add(value.Name, currency);
                }

                var currencyValue = new DAL.Context.CurrencyValue
                {
                    Date = value.Date,
                    Value = value.Value,
                    Currency = currency,
                    Amount = value.Amount,
                    CurrencyId = currency?.Id ?? 0
                };

                await _repository.MergeAsync(currencyValue, cv => new { cv.Date, cv.CurrencyId });
            }

            _repository.CommitTransaction();
        }
    }
}