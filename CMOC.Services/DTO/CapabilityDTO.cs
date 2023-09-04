namespace CMOC.Services.Dto;

public class CapabilityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ServiceDto> Dependencies { get; set; } = new List<ServiceDto>();
    public ObjectStatus Status { get; set; }
}