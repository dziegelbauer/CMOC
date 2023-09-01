namespace CMOC.Domain;

public class CapabilitySupportRelationship
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; }
    public int CapabilityId { get; set; }
    public Capability Capability { get; set; }
    public int RedundantWithId { get; set; }
    public ServiceRedundancy RedundantWith { get; set; }
}