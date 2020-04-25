using System;
using System.Threading.Tasks;

namespace Parser.Services
{
    public interface ICurrencySaveService
    {
        Task SaveByDate(DateTime date);
        Task SaveByYear(int year);
    }
}