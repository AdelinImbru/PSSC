using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public record Address
    {
        public string Value { get; }
        private static readonly Random random = new Random();

        public Address(string value)
        {
            if(random.Next(100)>50)
            {
                Value = value;
            }
            else
            {
                throw new InvalidAddressException($"{value} is an invalid address.");
            }
        }

        public override string ToString()
        {
            return $"Address: {Value}";
        }
    }
}
