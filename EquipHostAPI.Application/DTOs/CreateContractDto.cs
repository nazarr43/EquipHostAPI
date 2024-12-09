namespace EquipHostAPI.Application.DTOs;
public class CreateContractDto
{
    public string ProductionFacilityCode { get; set; } 
    public string EquipmentTypeCode { get; set; }
    public int EquipmentQuantity { get; set; }
}

