namespace CMOC.Domain;

public class Capability
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<CapabilitySupportRelationship> SupportedBy { get; set; } = new List<CapabilitySupportRelationship>();
}