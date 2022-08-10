using MinimalCleanApi.Contracts.DTO.Members;
using MinimalCleanApi.Domain;
using MinimalCleanApi.Domain.Common;
using MinimalCleanApi.Domain.Membership;

namespace MinimalCleanApi.Mappings;

public static class MembershipDomainDto
{
    public static Person ToPerson(this PersonDto personDto, IEnumerable<Relationship> relationships)
    {
        return new Person
        {
            ID = personDto.ID,
            FirstName = NamePart.From(personDto.FirstName),
            LastName = NamePart.From(personDto.LastName),
            Email = EmailAddress.From(personDto.Email),
            DateOfBirth = DateOfBirth.From(personDto.DateOfBirth),
            MembershipInformation = personDto.MembershipInformation.ToMembershipInformation(),
            Address = personDto.Address.ToAddress(),
            Relationships = relationships,
            AddedOn = personDto.AddedOn
        };
    }

    public static Address ToAddress(this AddressDto addressDto)
    {
        return new Address
        {
            ID = addressDto.ID,
            StreetLine1 = addressDto.StreetLine1,
            StreetLine2 = addressDto.StreetLine2,
            City = addressDto.City,
            State = (States)addressDto.State,
            PostalCode = addressDto.PostalCode
        };
    }

    public static MembershipInformation ToMembershipInformation(this MembershipInformationDto membershipInformationDto)
    {
        return new MembershipInformation
        {
            ID = membershipInformationDto.ID,
            DateOfMembership = DateOfMembership.From(membershipInformationDto.DateOfMembership),
            IsActive = membershipInformationDto.IsActive,
            MembershipStory = membershipInformationDto.MembershipStory,
            MembershipType = (MembershipType)membershipInformationDto.MembershipType
        };
    }
}