namespace CMOC.Domain;

public class Equipment
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }
    public List<ComponentRelationship> Components { get; set; }
    public int TypeId { get; set; }
    public EquipmentType Type { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
    public int? IssueId { get; set; }
    public Issue Issue { get; set; }
    public List<ServiceSupportRelationship> Relationships { get; set; }
    public string Notes { get; set; }
    public bool? OperationalOverride { get; set; }
}