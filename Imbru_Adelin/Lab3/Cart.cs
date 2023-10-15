using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    [AsChoice]
    public static partial class Cart
    {
        public interface ICart { }

        public record EmptyCart: ICart
        {
            public EmptyCart(IReadOnlyCollection<UnvalidatedProduct> productList)
            {
                ProductList = productList;
            }

            public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
        }

        public record InvalidatedCart: ICart
        {
            internal InvalidatedCart(IReadOnlyCollection<UnvalidatedProduct> productList, string reason)
            {
                ProductList = productList;
                Reason = reason;
            }

            public IReadOnlyCollection<UnvalidatedProduct> ProductList { get; }
            public string Reason { get; }
        }

        public record ValidatedCart: ICart
        {
            internal ValidatedCart(IReadOnlyCollection<ValidatedProduct> productsList)
            {
                ProductList = productsList;
            }

            public IReadOnlyCollection<ValidatedProduct> ProductList { get; }
        }

        public record CalculatedCart : ICart
        {
            internal CalculatedCart(IReadOnlyCollection<CalculatedFinalPrice> productsList)
            {
                ProductList = productsList;
            }

            public IReadOnlyCollection<CalculatedFinalPrice> ProductList { get; }
        }

        public record PayedCart : ICart
        {
            internal PayedCart(IReadOnlyCollection<CalculatedFinalPrice> productsList, string csv, DateTime paymentDate)
            {
                ProductList = productsList;
                PaymentDate = paymentDate;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedFinalPrice> ProductList { get; }
            public DateTime PaymentDate { get; }
            public string Csv { get; }
        }
    }
}
