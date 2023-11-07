using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public record Price
    {
        public float Value { get; }

        public Price(float value)
        {
            if (value>0 && value<1000)
            {
                Value = (float)value;         
            }
            else
            {
                throw new InvalidPriceException($"{value} is an invalid price value.");
            }
        }


        public override string ToString()
        {
            return $"Price: {Value}";
        }

        public static bool TryParsePrice(string priceString, out Price price)
        {
            bool isValid = false;
            price = null;
            if(float.TryParse(priceString, out float numericPrice))
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
