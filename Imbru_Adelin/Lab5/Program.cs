using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lab5
{
    class Program
    {
        private static readonly Random random = new Random();

        private static string ConnectionString = "Data Source=DESKTOP-B4V6BM2\\SQL2023;Initial Catalog=master;Integrated Security=True;Password=******;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Name=azdata;Command Timeout=30";

        static async Task Main(string[] args)
        {
            using ILoggerFactory loggerFactory = ConfigureLoggerFactory();
            ILogger<PayCartWorkflow> logger = loggerFactory.CreateLogger<PayCartWorkflow>();

            var listOfProducts = ReadListOfProducts().ToArray();
            PayCartCommand command = new(listOfProducts);
            var dbContextBuilder = new DbContextOptionsBuilder<OrderContext>()
                                                .UseSqlServer(ConnectionString)
                                                .UseLoggerFactory(loggerFactory);
            OrderContext orderContext = new OrderContext(dbContextBuilder.Options);
            ProductRepository productRepository = new(orderContext);
            OrderLineRepository orderLineRepository = new(orderContext);
            OrderHeaderRepository orderHeaderRepository = new(orderContext);
            PayCartWorkflow workflow = new(productRepository, orderLineRepository, orderHeaderRepository, logger);
            var result = await workflow.ExecuteAsync(command);

            result.Match(
                    whenCartPayFailedEvent: @event =>
                    {
                        Console.WriteLine($"Payment failed: {@event.Reason}");
                        return @event;
                    },
                    whenCartPaySuccededEvent: @event =>
                    {
                        Console.WriteLine($"Publish succeeded.");
                        Console.WriteLine(@event.Csv);
                        return @event;
                    }
                );
        }

        private static ILoggerFactory ConfigureLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
                                builder.AddSimpleConsole(options =>
                                {
                                    options.IncludeScopes = true;
                                    options.SingleLine = true;
                                    options.TimestampFormat = "hh:mm:ss ";
                                })
                                .AddProvider(new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()));
        }

        private static List<UnvalidatedProduct> ReadListOfProducts()
        {
            List<UnvalidatedProduct> listOfProducts = new();
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

                var price = ReadValue("Price: ");
                if (string.IsNullOrEmpty(price))
                {
                    break;
                }

                var address = ReadValue("Address: ");
                if (string.IsNullOrEmpty(address))
                {
                    break;
                }

                listOfProducts.Add(new(productCode, quantity, price, address));
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
