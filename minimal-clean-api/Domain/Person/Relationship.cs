namespace MinimalCleanApi.Domain;

public class Relationship
{
    public string ID { get; set; }
    public Person To { get; set; }
    public Person From { get; set; }
    public RelationshipType RelationshipType { get; set; }
}