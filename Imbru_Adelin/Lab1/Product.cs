using static Lab1.Quantity;

public record Product
    {
        public Product(string productCode, IQuantity quantity){
            ProductCode=productCode;
            Quantity=quantity;
        }
        public string ProductCode { get; set; }
        public IQuantity Quantity { get; set; }
    }