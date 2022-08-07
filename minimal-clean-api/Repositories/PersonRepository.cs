using MinimalCleanApi.Contracts.DTO.Members;
using MinimalCleanApi.Domain.Membership;
using MongoDB.Entities;

namespace MinimalCleanApi.Repositories;

public class PersonRepository : IPersonRepository
{
    //create person
    public async Task CreateOrUpdatePerson(PersonDto personToSave, CancellationToken cancellationToken)
    {
        await DB.SaveAsync(personToSave, cancellation: cancellationToken);
    }
    //get all persons
    public async Task<IEnumerable<PersonDto>> GetAllPersons(bool includeArchived = false,
        CancellationToken cancellationToken = default)
    {
        if(includeArchived)
            return await DB.Find<PersonDto>().ExecuteAsync(cancellationToken);

        return await DB.Find<PersonDto>()
            .Match(p => p.MembershipInformation.IsActive)
            .ExecuteAsync(cancellationToken);
    }
    //delete person
    public async Task<(bool isAcknowledged, long deletedCount)> DeletePerson(PersonDto personToDelete,
        CancellationToken cancellationToken)
    {
        var result = await DB.DeleteAsync<PersonDto>(personToDelete.ID, cancellation: cancellationToken);
        return (result.IsAcknowledged, result.DeletedCount);
    }

    public async Task<PersonDto> GetPersonById(string personId, CancellationToken cancellationToken)
    {
        return await DB.Find<PersonDto>()
            .Match(p => p.ID.Equals(personId, StringComparison.InvariantCultureIgnoreCase))
            .ExecuteFirstAsync(cancellationToken);
    }

    public async Task<IEnumerable<PersonDto>> GetAllPersonsByMembershipTypes(IList<int> membershipTypes, bool includeArchived, CancellationToken cancellationToken = default)
    {
        if(includeArchived)
        {
            return await DB.Find<PersonDto>()
                .Match(p => membershipTypes.Contains(p.MembershipInformation.MembershipType))
                .ExecuteAsync(cancellationToken);
        }
        return await DB.Find<PersonDto>()
            .Match(p => membershipTypes.Contains(p.MembershipInformation.MembershipType) && p.MembershipInformation.IsActive)
            .ExecuteAsync(cancellationToken);
    }

    public async Task<IEnumerable<PersonDto>> GetPersonByName(string name, bool includeArchive = default,
        CancellationToken cancellationToken = default)
    {
        if (includeArchive)
        {
            return await DB.Find<PersonDto>()
                .Match(p => p.FirstName.Contains(name, StringComparison.InvariantCultureIgnoreCase) ||
                            p.LastName.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                .ExecuteAsync(cancellationToken);
        }
        
        return await DB.Find<PersonDto>()
            .Match(p => (p.FirstName.Contains(name, StringComparison.InvariantCultureIgnoreCase) ||
                         p.LastName.Contains(name, StringComparison.InvariantCultureIgnoreCase)) &&
                        p.MembershipInformation.IsActive)
            .ExecuteAsync(cancellationToken);
    }

    public async Task<IEnumerable<PersonDto>> GetPersonsByIds(IEnumerable<string> personIds,
        CancellationToken cancellationToken)
    {
        return await DB.Find<PersonDto>().Match(p => personIds.Contains(p.ID)).ExecuteAsync(cancellationToken);
    }
}

public interface IPersonRepository
{
    /// <summary>
    /// If personToSave has an ID then it is updated, else a new entity is created
    /// </summary>
    /// <param name="personToSave"><see cref="PersonDto"/>Entity to update</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateOrUpdatePerson(PersonDto personToSave, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all persons in the DB
    /// </summary>
    /// <param name="includeArchived"><see cref="bool"/> when true the result set will include inactive memberships</param>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <returns>Returns <see cref="IEnumerable{T}"/> of <see cref="PersonDto"/></returns>
    Task<IEnumerable<PersonDto>> GetAllPersons(bool includeArchived = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes person from database
    /// </summary>
    /// <param name="personToDelete"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<(bool isAcknowledged, long deletedCount)> DeletePerson(PersonDto personToDelete, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns collection of persons from collection of IDs
    /// </summary>
    /// <param name="personIds">Collection of ID strings</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<PersonDto>> GetPersonsByIds(IEnumerable<string> personIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// gets person by the given ID if exists.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PersonDto> GetPersonById(string personId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all people by the membership type
    /// </summary>
    /// <param name="membershipTypes"><see cref="IList{T}"/> of <see cref="MembershipType"/> as <see cref="int"/></param>
    /// <param name="includeArchived"><see cref="bool"/> when true the result set will include inactive memberships</param>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    Task<IEnumerable<PersonDto>> GetAllPersonsByMembershipTypes(IList<int> membershipTypes, bool includeArchived = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for person by name. includes all name fields.
    /// </summary>
    /// <param name="name"><see cref="string"/> of part of the name</param>
    /// <param name="includeArchive">when true, inactive memberships will be returned</param>
    /// <param name="cancellationToken">the <see cref="CancellationToken"/></param>
    /// <returns><see cref="IEnumerable{T}"/> of all entries that contain some part of given string</returns>
    Task<IEnumerable<PersonDto>> GetPersonByName(string name, bool includeArchive = default,
        CancellationToken cancellationToken = default);
}