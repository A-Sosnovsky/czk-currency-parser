using System.Threading.Tasks;

namespace Parser.Services.Report
{
    public interface ICurrencyReportService
    {
        WeekReportDto GetMonthReport(int month, int year);
    }
}