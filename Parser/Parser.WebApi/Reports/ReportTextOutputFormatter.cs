using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Parser.WebApi.Reports
{
    public class ReportTextOutputFormatter : StringOutputFormatter
    {
        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return context.Object is MonthReport || base.CanWriteResult(context);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context.Object is MonthReport report)
            {
                var builder = new StringBuilder();
                Format(builder, report);
                await context.HttpContext.Response.WriteAsync(builder.ToString());
            }
            else
            {
                await base.WriteResponseBodyAsync(context, selectedEncoding);
            }
        }

        private static void Format(StringBuilder builder, MonthReport report)
        {
            builder.AppendLine($"Year: {report.Date.Year}, month: {report.Date.ToString("MMMM", CultureInfo.InvariantCulture)}");
            builder.AppendLine("Week periods:");
            foreach (var period in report.WeekPeriods)
            {
                builder.Append($"{period.WeekStartDay}..{period.WeekEndDay}: ");
                FormatCurrencies(builder, period);
                builder.AppendLine();
            }
        }

        private static void FormatCurrencies(StringBuilder builder, MonthReportWeekPeriod period)
        {
            foreach (var row in period.Rows)
            {
                builder.Append($"{row.CurrencyName} - max: {row.Max}, min: {row.Min}, median: {row.Median}; ");
            }
        }
    }
}