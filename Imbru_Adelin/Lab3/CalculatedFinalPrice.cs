using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public record CalculatedFinalPrice(ProductCode ProductCode, Price Price, Quantity Quantity, Address Address, int FinalPrice);
}
