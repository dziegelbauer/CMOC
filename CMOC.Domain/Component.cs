namespace CMOC.Domain;

public class Component
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public int TypeId { get; set; }
    public ComponentType Type { get; set; } = null!;
    public bool Operational { get; set; }
    public int ComponentOfId { get; set; }
    public ComponentRelationship ComponentOf { get; set; } = null!;
    public int? IssueId { get; set; }
    public Issue? Issue { get; set; }
}