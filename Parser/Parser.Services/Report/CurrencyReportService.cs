using System;
using System.Linq;
using Parser.DAL;
using Parser.DAL.Context;
using Parser.Common;

namespace Parser.Services.Report
{
    internal class CurrencyReportService : ICurrencyReportService
    {
        private readonly IRepository _repository;

        public CurrencyReportService(IRepository repository)
        {
            _repository = repository;
        }

        public WeekReportDto GetMonthReport(int month, int year)
        {
            var values = (from cv in _repository.Query<DAL.Context.CurrencyValue>()
                    join c in _repository.Query<Currency>() on cv.CurrencyId equals c.Id
                    where DbFunctionExtensions.DatePart("YEAR", cv.Date) == year &&
                          DbFunctionExtensions.DatePart("MONTH", cv.Date) == month
                          && new[]{1, 30, }.Contains(cv.CurrencyId)
                    select new
                    {
                        Date = cv.Date,
                        Currency = c.Name,
                        UnitValue = cv.UnitValue,
                        WeekNumber = DbFunctionExtensions.DatePart("WEEK", cv.Date).Value
                    }
                ).AsEnumerable()
                .GroupBy(r => new {WeekNumber = r.WeekNumber, r.Currency})
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