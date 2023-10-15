using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public record PayCartCommand
    {
        public PayCartCommand(IReadOnlyCollection<UnvalidatedProduct> inputCart)
        {
            InputCart = inputCart;
        }

        public IReadOnlyCollection<UnvalidatedProduct> InputCart { get; }
    }
}
