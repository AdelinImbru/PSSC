    namespace Lab2
    {
    public record Client
    {
        public Client(string name, string surname, string phoneNumber, Address address){
            Name=name;
            Surname=surname;
            PhoneNumber=phoneNumber;
            Address=address;
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
    }
}