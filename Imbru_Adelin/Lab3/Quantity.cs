using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
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
