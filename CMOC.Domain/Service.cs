namespace CMOC.Domain;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ServiceSupportRelationship> SupportedBy { get; set; }
    public List<CapabilitySupportRelationship> Supports { get; set; }
}