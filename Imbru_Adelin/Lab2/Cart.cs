using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2{
    [AsChoice]
    public static partial class Cart
    {
        public interface ICart { }

        public record EmptyCart(IReadOnlyCollection<UnvalidatedProduct> ProductsList) : ICart;

        public record InvalidatedCart(IReadOnlyCollection<UnvalidatedProduct> ProductsList, string reason) : ICart;

        public record ValidatedCart(IReadOnlyCollection<ValidatedProduct> ProductsList) : ICart;

        public record PayedCart(IReadOnlyCollection<ValidatedProduct> ProductsList, DateTime PublishedDate) : ICart;
    }
}
