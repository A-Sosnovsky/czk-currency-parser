using System.Collections.Generic;

namespace Parser.Services.Parsing
{
    public interface IParsingStrategy<out T>
    {
        IEnumerable<T> Parse(string input);
    }
}