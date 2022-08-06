namespace MinimalCleanApi.Domain;

public class Address
{
    public string StreetLine1 { get; set; }
    public string StreetLine2 { get; set; }
    public string City { get; set; }
    public States State { get; set; }
    public string PostalCode { get; set; }
}