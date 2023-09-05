namespace CMOC.Domain;

public class ServiceSupportRelationship
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public EquipmentType Type { get; set; } = null!;
    public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    public int FailureThreshold { get; set; }
    public int? RedundantWithId { get; set; }
    public EquipmentRedundancy? RedundantWith { get; set; }
}