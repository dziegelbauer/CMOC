namespace CMOC.Services.Dto;

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<EquipmentDto> Dependencies { get; set; } = new List<EquipmentDto>();
    public List<int> Dependents { get; set; } = new List<int>();
    public ObjectStatus Status { get; set; }
}