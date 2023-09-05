namespace CMOC.Domain;

public class ServiceRedundancy
{
    public int Id { get; set; }
    public List<CapabilitySupportRelationship> Redundancies { get; set; } = new List<CapabilitySupportRelationship>();
}