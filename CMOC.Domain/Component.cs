namespace CMOC.Domain;

public class Component
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }
    public int TypeId { get; set; }
    public ComponentType Type { get; set; }
    public bool Operational { get; set; }
    public int ComponentOfId { get; set; }
    public ComponentRelationship ComponentOf { get; set; }
}