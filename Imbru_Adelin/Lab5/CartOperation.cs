using static LanguageExt.Prelude;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab5.ProductCode;
using static Lab5.Cart;

namespace Lab5
{
    public static class CartOperation
        {
        public static Task<ICart> ValidateCart(Func<ProductCode, Option<ProductCode>> checkProductExists, EmptyCart cart) =>
            cart.ProductList
                      .Select(ValidateCustomerCart(checkProductExists))
                      .Aggregate(CreateEmptyValidatedProductList().ToAsync(), ReduceValidProduct)
                      .MatchAsync(
                            Right: validatedCart => new ValidatedCart(validatedCart),
                            LeftAsync: errorMessage => Task.FromResult((ICart)new InvalidatedCart(cart.ProductList, errorMessage))
                      );

        private static Func<UnvalidatedProduct, EitherAsync<string, ValidatedCart>> ValidateCustomerCart(Func<ProductCode, Option<ProductCode>> checkProductExists) =>
            unvalidatedProduct => ValidateCustomerCart(checkProductExists, unvalidatedProduct);

        private static EitherAsync<string, ValidatedProduct> ValidateCustomerCart(Func<ProductCode, Option<ProductCode>> checkProductExists, UnvalidatedProduct unvalidatedProduct)=>
            from productCode in ProductCode.TryParseProductCode(unvalidatedProduct.ProductCode, out ProductCode code)
                                   .ToEitherAsync($"Invalid student registration number ({unvalidatedProduct.ProductCode})")
            from quantity in Quantity.TryParseQuantity(unvalidatedProduct.Quantity, out Quantity quantity)
                                   .ToEitherAsync($"Invalid quantity ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Quantity})")
            from price in Price.TryParsePrice(unvalidatedProduct.Price, out Price price)
                                   .ToEitherAsync($"Invalid price ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Price})")
            from address in Address.TryParseAddress(unvalidatedProduct.Address, out Address address)
                                   .ToEitherAsync($"Invalid address ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Address})")
            from productExists in checkProductExists(productCode)
                                   .ToEitherAsync($"Product {productCode.Value} does not exist.")
            select new ValidatedProduct(productCode, quantity, price, address);

        private static Either<string, List<ValidatedProduct>> CreateEmptyValidatedProductList() =>
            Right(new List<ValidatedProduct>());

        private static EitherAsync<string, List<ValidatedProduct>> ReduceValidProduct(EitherAsync<string, List<ValidatedProduct>> acc, EitherAsync<string, ValidatedProduct> next) =>
            from list in acc
            from nextProduct in next
            select list.AppendValidProduct(nextProduct);

        private static List<ValidatedProduct> AppendValidProduct(this List<ValidatedProduct> list, ValidatedProduct validProduct)
        {
            list.Add(validProduct);
            return list;
        }

        public static ICart CalculateFinalPrice(ICart cart) => cart.Match(
            whenEmptyCart: emptyCart => emptyCart,
            whenInvalidatedCart: invalidCart => invalidCart,
            whenCalculatedCart: calculatedCart => calculatedCart,
            whenPaidCart: paidCart => paidCart,
            whenValidatedCart: CalculateFinalPrice
        );

        private static ICart CalculateFinalPrice(ValidatedCart validCart) =>
            new CalculatedCart(validCart.ProductList
                                                    .Select(CalculateCartFinalPrice)
                                                    .ToList()
                                                    .AsReadOnly());

        private static CalculatedFinalPrice CalculateCartFinalPrice(ValidatedProduct validProduct) => 
            new CalculatedFinalPrice(validProduct.ProductCode,
                                      validProduct.Price,
                                      validProduct.Quantity,
                                      validProduct.Address,
                                      validProduct.Price.Value*validProduct.Quantity.Value);

        public static ICart MergeProducts(ICart cart, IEnumerable<CalculatedFinalPrice> existingProducts) => cart.Match(
            whenEmptyCart: emptyCart => emptyCart,
            whenInvalidatedCart: invalidCart => invalidCart,
            whenValidatedCart: validatedCart => validatedCart,
            whenPaidCart: paidCart => paidCart,
            whenCalculatedCart: calculatedCart => MergeProducts(calculatedCart.ProductList, existingProducts));

        private static CalculatedCart MergeProducts(IEnumerable<CalculatedFinalPrice> newList, IEnumerable<CalculatedFinalPrice> existingList)
        {
            var updatedAndNewProducts = newList.Select(product => product with { CartId = existingList.FirstOrDefault(p => p.ProductCode == product.ProductCode)?.CartId ?? 0, IsUpdated = true });
            var oldProducts = existingList.Where(product => !newList.Any(p => p.ProductCode == product.ProductCode));
            var allProducts = updatedAndNewProducts.Union(oldProducts)
                                               .ToList()
                                               .AsReadOnly();
            return new CalculatedCart(allProducts);
        }

        public static ICart PayCart(ICart cart) => cart.Match(
            whenEmptyCart: emptyCart => emptyCart,
            whenInvalidatedCart: invalidCart => invalidCart,
            whenValidatedCart: validatedCart => validatedCart,
            whenPaidCart: paidCart => paidCart,
            whenCalculatedCart: GenerateExport);

        private static ICart GenerateExport(CalculatedCart calculatedCart) => 
            new PaidCart(calculatedCart.ProductList, 
                                    calculatedCart.ProductList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(), 
                                    DateTime.Now);

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedFinalPrice product) =>
            export.AppendLine($"{product.ProductCode.Value}, {product.Quantity}, {product.Price}, {product.FinalPrice}");
    }
}
