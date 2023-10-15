using Lab2;
using System;
using System.Collections.Generic;
using static Lab2.Cart;

namespace Lab2
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var listOfProducts = ReadListOfProducts().ToArray();
            EmptyCart emptyCart = new EmptyCart(listOfProducts);
            ICart result = ValidateCart(emptyCart);
            result.Match(
                whenEmptyCart: emptyResult => emptyCart,
                whenPayedCart: payedResult => payedResult,
                whenInvalidatedCart: invalidResult => invalidResult,
                whenValidatedCart: validatedResult => PayCart(validatedResult)
            );

        Console.WriteLine("Hello World!");
        }

        private static List<UnvalidatedProduct> ReadListOfProducts()
        {
            List <UnvalidatedProduct> listOfProducts = new();
            do
            {
                var productCode = ReadValue("Product code: ");
                if (string.IsNullOrEmpty(productCode))
                {
                    break;
                }

                var quantity = ReadValue("Quantity: ");
                if (string.IsNullOrEmpty(quantity))
                {
                    break;
                }

                listOfProducts.Add(new UnvalidatedProduct(productCode, quantity));
            } while (true);
            return listOfProducts;
        }

        private static ICart ValidateCart(EmptyCart emptyCart) =>
            random.Next(100) > 50 ?
            new InvalidatedCart(new List<UnvalidatedProduct>(), "Random errror")
            : new ValidatedCart(new List<ValidatedProduct>());

        private static ICart PayCart(ValidatedCart validCart) =>
            new PayedCart(new List<ValidatedProduct>(), DateTime.Now);

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
