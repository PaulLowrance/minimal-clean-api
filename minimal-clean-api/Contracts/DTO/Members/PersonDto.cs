using MongoDB.Entities;

namespace MinimalCleanApi.Contracts.DTO.Members;

public class PersonDto : Entity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string MembershipInformationId { get; set; } = default!;
    public string AddressId { get; set; } = default!;
    public DateTime AddedOn { get; set; }
    public DateTime Updated { get; set; }
}