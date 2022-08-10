using MinimalCleanApi.Contracts.DTO.Members;
using MinimalCleanApi.Domain;

namespace MinimalCleanApi.Mappings;

public static class RelationshipDomainDto
{
    public static RelationshipDto ToRelationshipDto(this Relationship relationship)
    {
        return new RelationshipDto
        {
            ID = relationship.ID,
            To = relationship.To.ID,
            From = relationship.From.ID,
            RelationshipType = (int)relationship.RelationshipType
        };
    }
}