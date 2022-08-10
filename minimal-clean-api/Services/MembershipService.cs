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

    public async Task DeleteMember(Person personToDelete)
    {
        var personDto = personToDelete.ToPersonDto();

        await _personRepository.DeletePerson(personDto);
        await _relationshipRepository.DeleteRelationships(personToDelete.Relationships.Select(r => r.ID));
    }

}

public interface IMembershipService
{
}