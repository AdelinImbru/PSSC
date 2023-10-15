    public record Order
    {
        public Order(Contact client, List<Product> products){
            Client=client;
            Products=products;
        }
        public Contact Client { get; set; }
        public List<Product> Products { get; set; }
    }