using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public record Price
    {
        public int Value { get; }
        public string Currency { get; }

        public Price(int value, string currency)
        {
            if (value>0 && value<1000)
            {
                Value = value;
                Currency = currency;
            }
            else
            {
                throw new InvalidPriceException($"{value} is an invalid price value.");
            }
        }

        public Price Round()
        {
            var roundedValue = Math.Round(Value);
            return new Price(roundedValue, Currency);
        }

        public override string ToString()
        {
            return $"Price: {Value} {Currency}";
        }
    }
}
