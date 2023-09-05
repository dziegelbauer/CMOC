namespace CMOC.Domain;

public class ComponentRelationship
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public ComponentType Type { get; set; } = null!;
    public List<Component> Components { get; set; } = new List<Component>();
    public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public int FailureThreshold { get; set; }
}