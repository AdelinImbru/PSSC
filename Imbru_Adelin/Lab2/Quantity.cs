using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public record Quantity
    {
        public decimal Value { get; }

        public Quantity(decimal value)
        {
            if (value>0 && value<100)
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
    }
}
