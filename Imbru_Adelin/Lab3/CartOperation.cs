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
                if (!Product.TryParseProduct(unvalidatedProduct.Cart, out Product cart))
                {
                    invalidReson = $"Invalid cart ({unvalidatedProduct.ProductCode}, {unvalidatedGrade.Cart})";
                    isValidList = false;
                    break;
                }
                if (!Product.TryParseProduct(unvalidatedProduct.ProductCode, out ProductCode productCode)
                    && checkProductCodeExists(productCode))
                {
                    invalidReson = $"Invalid product code ({unvalidatedProduct.ProductCode})";
                    isValidList = false;
                    break;
                }
                if (!Product.TryParseProduct(unvalidatedProduct.Quantity, out Product Quantity))
                {
                    invalidReson = $"Invalid quantity ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Quantity})";
                    isValidList = false;
                    break;
                }
                if (!Product.TryParseProduct(unvalidatedProduct.Price, out Product Price))
                {
                    invalidReson = $"Invalid price ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.Price})";
                    isValidList = false;
                    break;
                }
                if (!Product.TryParseProduct(unvalidatedProduct.Address, out Address address))
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
                return new ValidatedProduct(validatedProducts);
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
                                                                      validCart.Price.Value*validCart.Quantity.Value));
                return new CalculatedFinalPrice(calculatedCart.ToList().AsReadOnly());
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
