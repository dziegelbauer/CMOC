namespace CMOC.Services.Dto;

public class ComponentDto
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public int TypeId { get; set; }
    public string? Type { get; set; }
    public bool Operational { get; set; }
    public int ComponentOfId { get; set; }
    public int? IssueId { get; set; }
    public IssueDto? Issue { get; set; }
    public ObjectStatus Status { get; set; }
}