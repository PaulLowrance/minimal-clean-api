using MinimalCleanApi.Domain.Common;
using MinimalCleanApi.Domain.Membership;

namespace MinimalCleanApi.Domain;

public class Person
{
    public string ID { get; set; }
    public NamePart FirstName { get; set; } = default!;
    public NamePart LastName { get; set; } = default!;
    public EmailAddress Email { get; set; } = default!;
    public DateOfBirth DateOfBirth { get; set; } = default!;
    public MembershipInformation MembershipInformation { get; set; }
    public Address Address { get; set; }
    public IEnumerable<Relationship> Relationships { get; set; }
    public DateTime AddedOn { get; set; }
}
