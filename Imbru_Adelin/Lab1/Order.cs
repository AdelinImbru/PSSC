    public record Order
    {
        public Order(Contact client, List<Product> products){
            Client=client;
            Products=products;
        }
        public Contact Client { get; set; }
        public List<Product> Products { get; set; }

        public void print()
        {
            Console.WriteLine("Client=" + Client + "\n");
            foreach(Product product in Products){product.print();};
        }
    }