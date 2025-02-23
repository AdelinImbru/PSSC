using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab2
{
    public record ProductCode
    {
        private static readonly Regex ValidPattern = new("^10324[0-9]{5}$");

        public string Value { get; }

        private ProductCode(string value)
        {
            if (ValidPattern.IsMatch(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductCodeException("");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
