using EquipHostAPI.Application.DTOs;

namespace EquipHostAPI.Application.Interfaces;
public interface IContractService
{
    Task<int> CreateContractAsync(CreateContractDto dto);
    Task<IEnumerable<ContractDto>> GetAllContractsAsync();
}

