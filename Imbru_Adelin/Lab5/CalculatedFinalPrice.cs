using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public record CalculatedFinalPrice(ProductCode ProductCode, Price Price, Quantity Quantity, Address Address, float FinalPrice)
    {
        public int CartId { get; set; }
        public bool IsUpdated { get; set; }
    };
}
