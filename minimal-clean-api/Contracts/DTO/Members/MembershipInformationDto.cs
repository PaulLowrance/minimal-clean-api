using MongoDB.Entities;

namespace MinimalCleanApi.Contracts.DTO.Members;

public class MembershipInformationDto : Entity
{
    public int MembershipType { get; set; } = default!;
    public DateOnly DateOfMembership { get; set; } = default!;
    public string MembershipStory { get; set; } = default!;
    public bool IsActive { get; set; }
}