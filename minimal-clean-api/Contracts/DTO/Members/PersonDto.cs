using MinimalCleanApi.Domain;
using MongoDB.Entities;

namespace MinimalCleanApi.Contracts.DTO.Members;

public class PersonDto : Entity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; } = default!;
    public string Email { get; set; } = default!;
    public MembershipInformationDto MembershipInformation { get; set; } = default!;
    public AddressDto Address { get; set; }
    public DateTime AddedOn { get; set; }
    public DateTime Updated { get; set; }
}