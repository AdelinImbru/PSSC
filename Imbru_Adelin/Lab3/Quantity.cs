using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public record Quantity
    {
        public decimal Value { get; }
        private static readonly Random random = new Random();

        public Quantity(int value)
        {
            if (value>0 && value<random.Next(100))
            {
                Value = value;
            }
            else
            {
                throw new InvalidQuantityException($"{value:0.##} is an invalid quantity value.");
            }
        }

        public Quantity Round()
        {
            var roundedValue = Math.Round(Value);
            return new Quantity(roundedValue);
        }

        public override string ToString()
        {
            return $"Quantity: {Value}";
        }

        public static bool TryParseQuantity(string quantityString, out Quantity quantity)
        {
            bool isValid = false;
            quantity = null;
            if(decimal.TryParse(quantityString, out decimal numericQuantity))
            {
                if (numericQuantity>0 && numericQuantity<1000)
                {
                    isValid = true;
                    quantity = new(numericQuantity);
                }
            }

            return isValid;
        }
    }
}
