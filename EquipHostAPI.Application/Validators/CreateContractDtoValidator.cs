using EquipHostAPI.Application.DTOs;
using EquipHostAPI.Application.Interfaces;
using FluentValidation;

namespace EquipHostAPI.Application.Validators;
public class CreateContractDtoValidator : AbstractValidator<CreateContractDto>
{
    private readonly IContractRepository _contractRepository;

    public CreateContractDtoValidator(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;

        RuleFor(x => x.ProductionFacilityCode)
            .NotEmpty().WithMessage("Production Facility Code is required.");

        RuleFor(x => x.EquipmentTypeCode)
            .NotEmpty().WithMessage("Equipment Type Code is required.");

        RuleFor(x => x.EquipmentQuantity)
            .GreaterThan(0).WithMessage("Equipment Quantity must be greater than 0.");

        RuleFor(x => x)
            .MustAsync(HasEnoughAvailableSpace).WithMessage("Not enough available space to place the equipment.");
    }

    private async Task<bool> HasEnoughAvailableSpace(CreateContractDto contractDto, CancellationToken cancellationToken)
    {
        var facility = await _contractRepository.GetProductionFacilityByCodeAsync(contractDto.ProductionFacilityCode);
        var equipmentType = await _contractRepository.GetEquipmentTypeByCodeAsync(contractDto.EquipmentTypeCode);

        if (facility == null || equipmentType == null)
        {
            return false;
        }

        var requiredArea = contractDto.EquipmentQuantity * equipmentType.Area;

        return facility.StandardArea >= requiredArea;
    }
}

