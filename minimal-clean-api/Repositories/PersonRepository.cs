using MinimalCleanApi.Contracts.DTO.Members;
using MongoDB.Entities;

namespace MinimalCleanApi.Repositories;

public class PersonRepository : IPersonRepository
{
    //create person
    public async Task CreateOrUpdatePerson(PersonDto personToSave)
    {
        await DB.SaveAsync(personToSave);
    }
    //get all persons
    public async Task<IEnumerable<PersonDto>> GetAllPersons()
    {
        return await DB.Find<PersonDto>().ExecuteAsync();
    }
    //delete person
    public async Task<(bool isAcknowledged, long deletedCount)> DeletePerson(PersonDto personToDelete)
    {
        var result = await DB.DeleteAsync<PersonDto>(personToDelete.ID);
        return (result.IsAcknowledged, result.DeletedCount);
    }

    public async Task<PersonDto> GetPersonById(string personId)
    {
        return await DB.Find<PersonDto>()
            .Match(p => p.ID.Equals(personId, StringComparison.InvariantCultureIgnoreCase))
            .ExecuteFirstAsync();
    }

    public async Task<IEnumerable<PersonDto>> GetPersonsByIds(IEnumerable<string> personIds)
    {
        return await DB.Find<PersonDto>().Match(p => personIds.Contains(p.ID)).ExecuteAsync();
    }
}

public interface IPersonRepository
{
    /// <summary>
    /// If personToSave has an ID then it is updated, else a new entity is created
    /// </summary>
    /// <param name="personToSave"><see cref="PersonDto"/>Entity to update</param>
    /// <returns></returns>
    Task CreateOrUpdatePerson(PersonDto personToSave);
    /// <summary>
    /// Gets all persons in the DB
    /// </summary>
    /// <returns>Returns <see cref="IEnumerable{T}"/> of <see cref="PersonDto"/></returns>
    Task<IEnumerable<PersonDto>> GetAllPersons();
    /// <summary>
    /// Deletes person from database
    /// </summary>
    /// <param name="personToDelete"></param>
    /// <returns></returns>
    Task<(bool isAcknowledged, long deletedCount)> DeletePerson(PersonDto personToDelete);
    /// <summary>
    /// Returns collection of persons from collection of IDs
    /// </summary>
    /// <param name="personIds">Collection of ID strings</param>
    /// <returns></returns>
    Task<IEnumerable<PersonDto>> GetPersonsByIds(IEnumerable<string> personIds);
    /// <summary>
    /// gets person by the given ID if exists.
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    Task<PersonDto> GetPersonById(string personId);
}