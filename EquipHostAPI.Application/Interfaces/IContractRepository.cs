using EquipHostAPI.Application.DTOs;
using EquipHostAPI.Domain.Entities;

namespace EquipHostAPI.Application.Interfaces;
public interface IContractRepository
{
    Task<int> CreateContractAsync(CreateContractDto dto);
    Task<IEnumerable<ContractDto>> GetAllContractsAsync();
    Task<ProductionFacility> GetProductionFacilityByCodeAsync(string code);
    Task<EquipmentType> GetEquipmentTypeByCodeAsync(string code);
}

