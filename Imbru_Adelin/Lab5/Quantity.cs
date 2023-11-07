using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public record Quantity
    {
        public int Value { get; }
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

        public override string ToString()
        {
            return $"Quantity: {Value}";
        }

        public static bool TryParseQuantity(string quantityString, out Quantity quantity)
        {
            bool isValid = false;
            quantity = null;
            if(int.TryParse(quantityString, out int numericQuantity))
            {
                if (numericQuantity>0 && numericQuantity<1000)
                {
                    isValid = true;
                    quantity = new Quantity(numericQuantity);
                }
            }

            return isValid;
        }
    }
}
