namespace SliceByAlice.Models
{
    public class OrderDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Address2 { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public bool BillingAddressSameAsDelivery { get; set; }
        public string? BillingAddress { get; set; }
        public string? BillingAddress2 { get; set; }
        public string? BillingPostalCode { get; set; }
        public string? BillingCity { get; set; }
        public string? JSONCart { get; set; }
    }
}