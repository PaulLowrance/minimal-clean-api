using MinimalCleanApi.Contracts.DTO.Members;
using MinimalCleanApi.Domain;
using MinimalCleanApi.Mappings;
using MinimalCleanApi.Repositories;

namespace MinimalCleanApi.Services;

public class MembershipService : IMembershipService
{
    private IPersonRepository _personRepository;
    private IRelationshipRepository _relationshipRepository;
    private ILogger<IMembershipService> _logger;

    public MembershipService(IPersonRepository personRepository, IRelationshipRepository relationshipRepository, ILogger<MembershipService> logger)
    {
        _personRepository = personRepository;
        _relationshipRepository = relationshipRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gets all the active members in the system
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Person>> GetAllActiveMembers(CancellationToken cancellationToken = default)
    {
        var persons = new List<Person>();
        
        var memberDtos = await _personRepository.GetAllPersons();

        foreach (var personDto in memberDtos)
        {
            var relationshipDtos =
                await _relationshipRepository.GetRelationshipsForPerson(personDto.ID, cancellationToken: cancellationToken);
            var person = personDto.ToPerson(relationshipDtos.Select(r => r.ToRelationship()));
            persons.Add(person);
        }

        return persons;
    }

    /// <summary>
    /// Returns a Person by Id
    /// </summary>
    /// <param name="personId">string ID of the person</param>
    /// <param name="cancellation">the cancelation token</param>
    /// <returns>The person object with relationships (if any), returns empty person if Id returned no results</returns>
    public async Task<Person> GetPersonAsync(string personId, CancellationToken cancellation = default)
    {
        var personDto = await _personRepository.GetPersonById(personId, cancellation);
        var relationships = await _relationshipRepository.GetRelationshipsForPerson(personId, cancellation);

        relationships ??= Enumerable.Empty<RelationshipDto>();

        if(personDto == null)
            return new Person();

        var person = personDto.ToPerson(relationships.Select(r => r.ToRelationship()));
        return person;
    }

    /// <summary>
    /// Returns persons by fuzzy name search.
    /// This will search both First and Last name fields and is case insensitive
    /// </summary>
    /// <param name="name">name string to search for</param>
    /// <param name="includeArchive">When true, results will include inactive members</param>
    /// <param name="token">the <see cref="CancellationToken"/></param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="Person"/></returns>
    public async Task<IEnumerable<Person>> GetPersonByNameAsync(string name, bool includeArchive, CancellationToken token)
    {
        var persons = new List<Person>();
        var personsDto = await _personRepository.GetPersonByName(name, includeArchive, token);
        var relationshipsDto = await _relationshipRepository.GetRelationshipsForPersonIds(personsDto.Select(p => p.ID), token);

        foreach(var personDto in personsDto)
        {
            var relationships = relationshipsDto.Select(r => r.ToRelationship());
            var person = personDto.ToPerson(relationships);
            persons.Add(person);
        }
        return persons;
    }


    /// <summary>
    /// Saves the person and their relationships
    /// </summary>
    /// <param name="personToSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SavePerson(Person personToSave, CancellationToken cancellationToken = default)
    {
        var personDto = personToSave.ToPersonDto();
        var relationshipDtos = personToSave.Relationships.Select(r => r.ToRelationshipDto());

        await _personRepository.CreateOrUpdatePerson(personDto, cancellationToken);
        await _relationshipRepository.CreateOrUpdateRelationships(relationshipDtos, cancellationToken);
    }






    public async Task DeleteMember(Person personToDelete)
    {
        var personDto = personToDelete.ToPersonDto();

        await _personRepository.DeletePerson(personDto);
        await _relationshipRepository.DeleteRelationships(personToDelete.Relationships.Select(r => r.ID));
    }

}

public interface IMembershipService
{
    /// <summary>
    /// Gets all the active members in the system
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Person>> GetAllActiveMembers(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a Person by Id
    /// </summary>
    /// <param name="personId">string ID of the person</param>
    /// <param name="cancellation">the cancelation token</param>
    /// <returns>The person object with relationships</returns>
    Task<Person> GetPersonAsync(string personId, CancellationToken cancellation = default);

    /// <summary>
    /// Returns persons by fuzzy name search.
    /// This will search both First and Last name fields and is case insensitive
    /// </summary>
    /// <param name="name">name string to search for</param>
    /// <param name="includeArchive">When true, results will include inactive members</param>
    /// <param name="token">the <see cref="CancellationToken"/></param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="Person"/></returns>
    Task<IEnumerable<Person>> GetPersonByNameAsync(string name, bool includeArchive, CancellationToken token);
}