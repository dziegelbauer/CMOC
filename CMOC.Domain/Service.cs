namespace CMOC.Domain;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ServiceSupportRelationship> SupportedBy { get; set; } = new List<ServiceSupportRelationship>();
    public List<CapabilitySupportRelationship> Supports { get; set; } = new List<CapabilitySupportRelationship>();
}