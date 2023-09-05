namespace CMOC.Domain;

public class EquipmentRedundancy
{
    public int Id { get; set; }
    public List<ServiceSupportRelationship> Redundancies { get; set; } = new List<ServiceSupportRelationship>();
}