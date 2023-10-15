    public record Contact
    {
        public Contact(string name, string surname, string phoneNumber, string address){
            Name=name;
            Surname=surname;
            PhoneNumber=phoneNumber;
            Address=address;
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }