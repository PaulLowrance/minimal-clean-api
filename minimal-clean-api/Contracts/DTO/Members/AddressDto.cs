using MongoDB.Entities;

namespace MinimalCleanApi.Contracts.DTO.Members;

public class AddressDto : Entity
{
    public string StreetLine1 { get; set; } = default!;
    public string StreetLine2 { get; set; } = default!;
    public string City { get; set; } = default!;
    public int State { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
}