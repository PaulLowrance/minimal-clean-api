using MinimalCleanApi.Contracts.DTO.Members;
using MinimalCleanApi.Domain;
using MongoDB.Entities;

namespace MinimalCleanApi.Repositories;

public interface IRelationshipRepository
{
    /// <summary>
    /// Saves the complete entity if it exists. If the ID is empty a new entity is created, else it is replaced
    /// </summary>
    /// <param name="relationshipDto">Entity to save</param>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task CreateOrUpdateRelationship(RelationshipDto relationshipDto, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a single relationship by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<RelationshipDto> GetRelationshipById(string id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a persons relationships by type. e.g. 'All Bob's children', or 'Jimmy's Parents'
    /// </summary>
    /// <param name="personId">Database ID of the person</param>
    /// <param name="relationshipType"><see cref="RelationshipType"/> as an int</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<RelationshipDto>> GetRelationshipsByPersonAndType(string personId,
        int relationshipType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes given relationship
    /// </summary>
    /// <param name="relationshipDto">the <see cref="RelationshipDto"/> to delete</param>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <returns><see cref="Tuple{T1, T2}"/> indicating the request was acknowledged and number of records affected</returns>
    Task<(bool isAcknowledged, long deleteCount)> DeleteRelationship(RelationshipDto relationshipDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the next level of relationships for a person
    /// </summary>
    /// <param name="personId">Database ID for the person</param>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="RelationshipDto"/></returns>
    Task<IEnumerable<RelationshipDto>> GetRelationshipsForPerson(string personId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a collection for relationships. Used to delete all relationships for a person
    /// </summary>
    /// <param name="relationshipsToDelete"></param>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <returns><see cref="Tuple{T1, T2}"/> indicating the request was acknowledged and number of records affected</returns>
    Task<(bool isAcknowledged, long deleteCount)> DeleteRelationships(IEnumerable<string> relationshipsToDelete,
        CancellationToken cancellationToken = default);
}

public class RelationshipRepository : IRelationshipRepository
{
    public async Task CreateOrUpdateRelationship(RelationshipDto relationshipDto, CancellationToken cancellationToken)
    {
        await DB.SaveAsync(relationshipDto, cancellation: cancellationToken);
    }

    public async Task<RelationshipDto> GetRelationshipById(string id, CancellationToken cancellationToken)
    {
        return await DB.Find<RelationshipDto>()
            .Match(r => r.ID.Equals(id, StringComparison.InvariantCultureIgnoreCase))
            .ExecuteSingleAsync(cancellationToken);
    }

    public async Task<IEnumerable<RelationshipDto>> GetRelationshipsByPersonAndType(string personId,
        int relationshipType, CancellationToken cancellationToken)
    {
        return await DB.Find<RelationshipDto>()
            .Match(r => r.From.Equals(personId, StringComparison.InvariantCultureIgnoreCase) &&
                        r.RelationshipType.Equals(relationshipType))
            .ExecuteAsync(cancellationToken);
    }

    public async Task<(bool isAcknowledged, long deleteCount)> DeleteRelationship(RelationshipDto relationshipDto,
        CancellationToken cancellationToken)
    {
        var result = await DB.DeleteAsync<RelationshipDto>(relationshipDto.ID, cancellation: cancellationToken);
        return (result.IsAcknowledged, result.DeletedCount);
    }

    public async Task<IEnumerable<RelationshipDto>> GetRelationshipsForPerson(string personId, CancellationToken cancellationToken)
    {
        return await DB.Find<RelationshipDto>()
            .Match(r => r.From.Equals(personId))
            .ExecuteAsync(cancellationToken);
    }

    public async Task<(bool isAcknowledged, long deleteCount)> DeleteRelationships(IEnumerable<string> relationshipsToDelete, CancellationToken cancellationToken = default)
    {
        var result = await DB.DeleteAsync<RelationshipDto>(relationshipsToDelete, cancellation: cancellationToken);
        return (result.IsAcknowledged, result.DeletedCount);
    }
}