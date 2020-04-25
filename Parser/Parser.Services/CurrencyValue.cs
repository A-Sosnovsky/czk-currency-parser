using System;

namespace Parser.Services
{
    internal class CurrencyValue
    {
        private string _name;
        private decimal _value;
        private int _amount;
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

        public int Amount
        {
            get => _amount;
            set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException(@"Value can't be less than zero");
                }

                _amount = value;
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
            return HashCode.Combine(Name, Value, Amount, Date);
        }

        private bool Equals(CurrencyValue obj)
        {
            return obj.Amount == _amount && obj.Value == _value && obj.Name == Name && obj.Date == Date;
        }
    }
}