namespace CMOC.Domain;

public class CapabilitySupportRelationship
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    public int CapabilityId { get; set; }
    public Capability Capability { get; set; } = null!;
    public int? RedundantWithId { get; set; }
    public ServiceRedundancy? RedundantWith { get; set; }
}