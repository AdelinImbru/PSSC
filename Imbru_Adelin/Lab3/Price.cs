using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public record Price
    {
        public decimal Value { get; }

        public Price(int value)
        {
            if (value>0 && value<1000)
            {
                Value = value;         }
            else
            {
                throw new InvalidPriceException($"{value} is an invalid price value.");
            }
        }

        public Price Round()
        {
            var roundedValue = Math.Round(Value);
            return new Price(roundedValue);
        }

        public override string ToString()
        {
            return $"Price: {Value}";
        }

        public static bool TryParsePrice(string priceString, out Price price)
        {
            bool isValid = false;
            price = null;
            if(decimal.TryParse(priceString, out decimal numericPrice))
            {
                if (numericPrice>0 && numericPrice<1000)
                {
                    isValid = true;
                    price = new(numericPrice);
                }
            }

            return isValid;
        }

    }
}
