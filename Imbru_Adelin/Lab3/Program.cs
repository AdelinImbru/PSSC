using System;
using System.Collections.Generic;

namespace Lab3
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var listOfProducts = ReadListOfProducts().ToArray();
            PayCartCommand command = new(listOfProducts);
            PayCartWorkflow workflow = new PayCartWorkflow();
            var result = workflow.Execute(command, (ProductCode) => true);

            result.Match(
                    whenCartPayFailedEvent: @event =>
                    {
                        Console.WriteLine($"Payment failed: {@event.Reason}");
                        return @event;
                    },
                    whenCartPaySucceededEvent: @event =>
                    {
                        Console.WriteLine($"Payment succeeded.");
                        Console.WriteLine(@event.Csv);
                        return @event;
                    }
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

                var price = ReadValue("Price: ");
                if (string.IsNullOrEmpty(price))
                {
                    break;
                }

                var quantity = ReadValue("Quantity: ");
                if (string.IsNullOrEmpty(quantity))
                {
                    break;
                }
                
                var address = ReadValue("Address: ");
                if (string.IsNullOrEmpty(address))
                {
                    break;
                }

                listOfProducts.Add(new (productCode, price, quantity, address));
            } while (true);
            return listOfProducts;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
