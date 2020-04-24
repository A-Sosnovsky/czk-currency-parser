using System;

namespace Parser.Services
{
    internal class CurrencyValue
    {
        private string _name;
        private decimal _value;

        
        public decimal Value
        {
            get => _value;
            set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException(@"Value can't be less than zero");
                }
                _value = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException(@"Value can't ve null or empty");
                }

                _name = value;
            }
        }
    }
}