using System;

namespace Parser.Services
{
    internal class CurrencyValue
    {
        private string _name;
        private decimal _value;
        private int _multiplier;
        
        public DateTime Date { get; set; }
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

        public int Multiplier
        {
            get => _multiplier;
            set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException(@"Value can't be less than zero");
                }

                _multiplier = value;
            }
        }


        public override bool Equals(object obj)
        {
            if(ReferenceEquals(null, obj))
            {
                return false;
            }            
            
            if(ReferenceEquals(this, obj))
            {
                return true;
            }
            
            return obj is CurrencyValue value && Equals(value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value, Multiplier, Date);
        }

        private bool Equals(CurrencyValue obj)
        {
            return obj.Multiplier == _multiplier && obj.Value == _value && obj.Name == Name && obj.Date == Date;
        }
    }
}