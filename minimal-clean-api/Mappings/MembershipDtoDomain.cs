using MinimalCleanApi.Contracts.DTO.Members;
using MinimalCleanApi.Domain;
using MinimalCleanApi.Domain.Membership;

namespace MinimalCleanApi.Mappings;

public static class MembershipDtoDomain
{
    public static PersonDto ToPersonDto(this Person person)
    {
        return new PersonDto
        {
            ID = person.ID,
            AddedOn = person.AddedOn,
            Address = person.Address.ToAddressDto(),
            DateOfBirth = person.DateOfBirth.Value,
            Email = person.Email.Value,
            FirstName = person.FirstName.Value,
            LastName = person.LastName.Value,
            MembershipInformation = person.MembershipInformation.ToMembershipInformationDto()
        };
    }

    public static AddressDto ToAddressDto(this Address address)
    {
        return new AddressDto
        {
            City = address.City,
            ID = address.ID,
            PostalCode = address.PostalCode,
            State = (int)address.State,
            StreetLine1 = address.StreetLine1,
            StreetLine2 = address.StreetLine2
        };
    }

    public static MembershipInformationDto ToMembershipInformationDto(this MembershipInformation membershipInformation)
    {
        return new MembershipInformationDto
        {
            ID = membershipInformation.ID,
            MembershipType = (int)membershipInformation.MembershipType,
            DateOfMembership = membershipInformation.DateOfMembership.Value,
            MembershipStory = membershipInformation.MembershipStory,
            IsActive = membershipInformation.IsActive
        };
    }
}