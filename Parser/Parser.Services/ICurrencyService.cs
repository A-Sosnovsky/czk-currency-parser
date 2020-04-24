using System;
using System.Threading.Tasks;

namespace Parser.Services
{
    public interface ICurrencyService
    {
        Task ParseByDate(DateTime date);
        Task ParseByYear(int year);
    }
}