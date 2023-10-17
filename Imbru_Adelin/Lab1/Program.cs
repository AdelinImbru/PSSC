using static Lab1.Quantity;

namespace Lab1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IQuantity quantity = ConvertToQuantity("2");
            quantity.Match(
                whenKilograms: kilograms => kilograms,
                whenUndefined: undefined => undefined,
                whenUnits: units => print(units));
            Console.WriteLine(quantity.GetType());
            Contact person = new Contact("Dan", "Ion", "0727421423", "Aleea Studentilor 22");
            Product product = new Product("12313213", quantity);
            List<Product> products = new List<Product>();
            products.Add(new Product("12313213", quantity));
            products.Add(new Product("51325325", quantity));
            Order order = new Order(person, products);
            order.print();
        }

        private static IQuantity ConvertToQuantity(string q){
            if (Int32.TryParse(q, out int units))
            return new Units(units);
            else if (Double.TryParse(q, out double kilograms))
            return new Kilograms(kilograms);
            else return new Undefined(q);
        }

        private static Units print(Units units){
            Console.WriteLine(units.number + 'u');
            return units;
        }
    }
}