using System.Collections.Generic;

namespace Parser.Services.Parsing
{
    internal interface IParser<out T>
    {
        IEnumerable<T> Parse(string input);
    }
}