namespace EquipHostAPI.Domain.Entities;
public class EquipmentPlacementContract
{
    public int Id { get; set; }
    public int ProductionFacilityId { get; set; }
    public int EquipmentTypeId { get; set; }
    public int EquipmentQuantity { get; set; }

    public ProductionFacility ProductionFacility { get; set; }
    public EquipmentType EquipmentType { get; set; }
}

