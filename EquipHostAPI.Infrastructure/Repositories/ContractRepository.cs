using EquipHostAPI.Application.DTOs;
using EquipHostAPI.Application.Interfaces;
using EquipHostAPI.Domain.Entities;
using EquipHostAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EquipHostAPI.Infrastructure.Repositories;
public class ContractRepository : IContractRepository
{
    private readonly AppDbContext _dbContext;

    public ContractRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateContractAsync(CreateContractDto dto)
    {
        var facility = await _dbContext.ProductionFacilities
            .FirstOrDefaultAsync(pf => pf.Code == dto.ProductionFacilityCode);

        if (facility == null)
        {
            throw new InvalidOperationException($"Production facility with code {dto.ProductionFacilityCode} not found.");
        }

        var equipmentType = await _dbContext.EquipmentTypes
            .FirstOrDefaultAsync(et => et.Code == dto.EquipmentTypeCode);
        if (equipmentType == null)
        {
            throw new InvalidOperationException($"Equipment type with code {dto.EquipmentTypeCode} not found.");
        }

        var contract = new EquipmentPlacementContract
        {
            ProductionFacilityId = facility.Id,
            EquipmentTypeId = equipmentType.Id,
            EquipmentQuantity = dto.EquipmentQuantity
        };

        _dbContext.EquipmentPlacementContracts.Add(contract);
        await _dbContext.SaveChangesAsync();
        return contract.Id;
    }

    public async Task<IEnumerable<ContractDto>> GetAllContractsAsync()
    {
        return await _dbContext.EquipmentPlacementContracts
            .Include(c => c.ProductionFacility)
            .Include(c => c.EquipmentType)
            .Select(c => new ContractDto
            {
                ProductionFacilityName = c.ProductionFacility.Name,
                EquipmentTypeName = c.EquipmentType.Name,
                EquipmentQuantity = c.EquipmentQuantity
            }).ToListAsync();
    }

    public async Task<ProductionFacility> GetProductionFacilityByCodeAsync(string code)
    {
        return await _dbContext.ProductionFacilities
            .FirstOrDefaultAsync(pf => pf.Code == code);
    }

    public async Task<EquipmentType> GetEquipmentTypeByCodeAsync(string code)
    {
        return await _dbContext.EquipmentTypes
            .FirstOrDefaultAsync(et => et.Code == code);
    }
}

