namespace CMOC.Domain;

public class ComponentRelationship
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public ComponentType Type { get; set; }
    public List<Component> Components { get; set; }
    public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; }
    public int FailureThreshold { get; set; }
}