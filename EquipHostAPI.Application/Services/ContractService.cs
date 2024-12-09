using EquipHostAPI.Application.DTOs;
using EquipHostAPI.Application.Interfaces;
using EquipHostAPI.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EquipHostAPI.Application.Services;
public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly IContractBackgroundProcessor _backgroundProcessor;
    private readonly ILogger<ContractService> _logger;
    private readonly IValidator<CreateContractDto> _validator;

    public ContractService(
        IContractRepository contractRepository, 
        IContractBackgroundProcessor backgroundProcessor, 
        ILogger<ContractService> logger,
        IValidator<CreateContractDto> validator)
    {
        _contractRepository = contractRepository;
        _backgroundProcessor = backgroundProcessor;
        _logger = logger;
        _validator = validator;
    }

    public async Task<int> CreateContractAsync(CreateContractDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var contractId = await _contractRepository.CreateContractAsync(dto);
        _logger.LogInformation($"Contract {contractId} created successfully.");

        _backgroundProcessor.ProcessContractCreated(contractId);

        return contractId;
    }
    public async Task<IEnumerable<ContractDto>> GetAllContractsAsync()
    {
        return await _contractRepository.GetAllContractsAsync();
    }
}

