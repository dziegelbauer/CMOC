namespace CMOC.Services.Dto;

public class EquipmentDto
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public List<ComponentDto> Components { get; set; } = new List<ComponentDto>();
    public int TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public string Location { get; set; } = string.Empty;
    public List<int> SupportedServices { get; set; } = new List<int>();
    public string? Notes { get; set; }
    public bool? OperationalOverride { get; set; }
    public int? IssueId { get; set; }
    public IssueDto? Issue { get; set; }
    public ObjectStatus Status { get; set; }
}