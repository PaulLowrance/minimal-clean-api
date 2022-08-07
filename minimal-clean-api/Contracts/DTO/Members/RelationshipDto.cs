using MongoDB.Entities;

namespace MinimalCleanApi.Contracts.DTO.Members;

public class RelationshipDto : Entity
{
    public string To { get; set; } = default!;
    public string From { get; set; } = default!;
    public int RelationshipType { get; set; } = default!;
}