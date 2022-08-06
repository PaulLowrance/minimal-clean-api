namespace MinimalCleanApi.Domain.Membership;

public class MembershipInformation
{
    public MembershipInformation()
    {
        MembershipType = MembershipType.Other;
    }
    
    public MembershipType MembershipType { get; set; }
    public DateOfMembership DateOfMembership { get; set; } = default!;
    public string MembershipStory { get; set; } = default!;
    public bool IsActive { get; set; }
}