namespace CMOC.Domain;

public class Capability
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<CapabilitySupportRelationship> SupportedBy { get; set; }
}