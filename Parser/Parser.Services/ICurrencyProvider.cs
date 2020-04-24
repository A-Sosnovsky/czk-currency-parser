using System;
using System.Threading.Tasks;

namespace Parser.Services
{
    public interface ICurrencyProvider
    {
        Task<string> GetByDate(DateTime date);
        Task<string> GetByYear(int year);
    }
}