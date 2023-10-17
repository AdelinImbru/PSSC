using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab3.Cart;

namespace Lab3
{
    public static class CartOperation
    {
        public static ICart ValidateCart(Func<ProductCode, bool> checkProductExists, EmptyCart cart)
        {
            List<ValidatedProduct> validatedProducts = new();
            bool isValidList = true;
            string invalidReson = string.Empty;
            foreach(var unvalidatedProduct in cart.ProductList)
            {
                if (!ProductCode.TryParseProductCode(unvalidatedProduct.ProductCode, out ProductCode productCode)
                    && checkProductExists(productCode))
                {
                    invalidReson = $"Invalid product code ({unvalidatedProduct.ProductCode})";
                    isValidList = false;
                    break;
                }
                if (!Quantity.TryParseQuantity(unvalidatedProduct.Quantity, out Quantity quantity))
                {
                    invalidReson = $"Invalid quantity ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Quantity})";
                    isValidList = false;
                    break;
                }
                if (!Price.TryParsePrice(unvalidatedProduct.Price, out Price price))
                {
                    invalidReson = $"Invalid price ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Price})";
                    isValidList = false;
                    break;
                }
                if (!Address.TryParseAddress(unvalidatedProduct.Address, out Address address))
                {
                    invalidReson = $"Invalid address ({unvalidatedProduct.Address})";
                    isValidList = false;
                    break;
                }
                ValidatedProduct validCart = new(productCode, quantity, price, address);
                validatedProducts.Add(validCart);
            }

            if (isValidList)
            {
                return new ValidatedCart(validatedProducts);
            }
            else
            {
                return new InvalidatedCart(cart.ProductList, invalidReson);
            }

        }

        public static ICart CalculateFinalPrice(ICart cart) => cart.Match(
            whenEmptyCart: emptyCart => emptyCart,
            whenInvalidatedCart: invalidCart => invalidCart,
            whenCalculatedCart: calculatedCart => calculatedCart,
            whenPayedCart: payedCart => payedCart,
            whenValidatedCart: validCart =>
            {
                var calculatedCart = validCart.ProductList.Select(validCart =>
                                            new CalculatedFinalPrice(validCart.ProductCode,
                                                                      validCart.Price,
                                                                      validCart.Quantity,
                                                                      validCart.Address,
                                                                      decimal.Multiply(validCart.Price.Value, validCart.Quantity.Value)));
                return new CalculatedCart(calculatedCart.ToList().AsReadOnly());
            }
        );

        public static ICart PayCart(ICart cart) => cart.Match(
            whenEmptyCart: emptyCart => emptyCart,
            whenInvalidatedCart: invalidCart => invalidCart,
            whenValidatedCart: validatedCart => validatedCart,
            whenPayedCart: payedCart => payedCart,
            whenCalculatedCart: calculatedCart =>
            {
                StringBuilder csv = new();
                calculatedCart.ProductList.Aggregate(csv, (export, cart) => export.AppendLine($"{cart.ProductCode.Value}, {cart.Quantity}, {cart.Price}, {cart.Address}, {cart.FinalPrice}"));

                PayedCart payedCart = new(calculatedCart.ProductList, csv.ToString(), DateTime.Now);

                return payedCart;
            });
    }
}
