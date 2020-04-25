using System.Collections.Generic;

namespace Parser.Services.Parsing
{
    internal class Parser<T> : IParser<T>
    {
        private readonly IParsingStrategy<T> _strategy;

        public Parser(IParsingStrategy<T> strategy)
        {
            _strategy = strategy;
        }

        public IEnumerable<T> Parse(string input)
        {
            return _strategy.Parse(input);
        }
    }
}