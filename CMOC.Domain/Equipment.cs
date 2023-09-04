namespace CMOC.Domain;

public class Equipment
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public List<ComponentRelationship> Components { get; set; } = new List<ComponentRelationship>();
    public int TypeId { get; set; }
    public EquipmentType Type { get; set; } = null!;
    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;
    public int? IssueId { get; set; }
    public Issue? Issue { get; set; }
    public List<ServiceSupportRelationship> Relationships { get; set; } = new List<ServiceSupportRelationship>();
    public string Notes { get; set; } = string.Empty;
    public bool? OperationalOverride { get; set; }
}