using MinimalCleanApi.Contracts.DTO.Members;
using MinimalCleanApi.Domain;

namespace MinimalCleanApi.Mappings;

public static class RelationshipDtoDomain
{
    public static Relationship ToRelationship(this RelationshipDto relationshipDto)
    {
        //todo: implement this 
        return new Relationship
        {
            To = null,
            From = null,
            RelationshipType = RelationshipType.GrandParent
        };
    }
}