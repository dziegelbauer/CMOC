namespace CMOC.Domain;

public class ServiceSupportRelationship
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public EquipmentType Type { get; set; }
    public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; }
    public int FailureThreshold { get; set; }
    public int? RedundantWithId { get; set; }
    public EquipmentRedundancy RedundantWith { get; set; }
}