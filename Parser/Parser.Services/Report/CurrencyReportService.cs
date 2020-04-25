using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Parser.DAL;
using Parser.DAL.Context;
using Parser.Common;

namespace Parser.Services.Report
{
    internal class CurrencyReportService : ICurrencyReportService
    {
        private const char Divider = ';';
        private const string ConfigurationReportKeyPrefix = "MonthReport";
        private const string ConfigurationCurrenciesKey = "Currencies";

        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;

        public CurrencyReportService(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public WeekReportDto GetMonthReport(int month, int year)
        {
            var currenciesForReport =
                _configuration.GetSection(ConfigurationReportKeyPrefix)[ConfigurationCurrenciesKey];

            var currencyNames = !string.IsNullOrWhiteSpace(currenciesForReport)
                ? currenciesForReport.Split(Divider, StringSplitOptions.RemoveEmptyEntries)
                : Array.Empty<string>();

            var values = (from cv in _repository.Query<DAL.Context.CurrencyValue>()
                    join c in _repository.Query<Currency>() on cv.CurrencyId equals c.Id
                    where DbFunctionExtensions.DatePart("YEAR", cv.Date) == year &&
                          DbFunctionExtensions.DatePart("MONTH", cv.Date) == month
                          && (currencyNames.IsNullOrEmpty() || currencyNames.Contains(c.Name))
                          
                    select new
                    {
                        Date = cv.Date,
                        Currency = c.Name,
                        UnitValue = cv.UnitValue,
                        WeekNumber = DbFunctionExtensions.DatePart("WEEK", cv.Date).Value
                    }
                ).AsEnumerable()
                .GroupBy(r => new {r.WeekNumber, r.Currency})
                .Select(grouped => new WeekCurrencyValueDto
                {
                    WeekNumber = grouped.Key.WeekNumber,
                    StartDay = grouped.Min(r => r.Date).Day,
                    EndDay = grouped.Max(r => r.Date).Day,
                    CurrencyName = grouped.Key.Currency,
                    MaxValue = grouped.Max(r => r.UnitValue),
                    MinValue = grouped.Min(r => r.UnitValue),
                    MedianValue = grouped.Select(r => r.UnitValue).Median(),
                });

            return new WeekReportDto
            {
                Date = new DateTime(year, month, 1),
                Weeks = values
            };
        }
    }
}