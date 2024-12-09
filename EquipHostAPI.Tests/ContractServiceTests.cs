using System.Collections.Generic;
using System.Threading.Tasks;
using EquipHostAPI.Application.DTOs;
using EquipHostAPI.Application.Interfaces;
using EquipHostAPI.Application.Services;
using EquipHostAPI.Application.Validators;
using FluentValidation;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using EquipHostAPI.Application;

namespace EquipHostAPI.Tests
{
    public class ContractServiceTests
    {
        private readonly Mock<IContractRepository> _contractRepositoryMock;
        private readonly Mock<IContractBackgroundProcessor> _backgroundProcessorMock;
        private readonly Mock<ILogger<ContractService>> _loggerMock;
        private readonly Mock<IValidator<CreateContractDto>> _validatorMock; 
        private readonly ContractService _contractService;

        public ContractServiceTests()
        {
            _contractRepositoryMock = new Mock<IContractRepository>();
            _backgroundProcessorMock = new Mock<IContractBackgroundProcessor>();
            _loggerMock = new Mock<ILogger<ContractService>>();
            _validatorMock = new Mock<IValidator<CreateContractDto>>(); 
            _contractService = new ContractService(
                _contractRepositoryMock.Object,
                _backgroundProcessorMock.Object,
                _loggerMock.Object,
                _validatorMock.Object
            );
        }

        

        [Fact]
        public async Task CreateContractAsync_ShouldThrowValidationException_WhenInvalidDto()
        {
            var dto = new CreateContractDto
            {
                ProductionFacilityCode = "FAC123",
                EquipmentTypeCode = "EQUIP456",
                EquipmentQuantity = 10
            };

            var validationFailures = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("ProductionFacilityCode", "Production Facility Code is required.")
            };

            var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);
            _validatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(validationResult);

            await Assert.ThrowsAsync<ValidationException>(() => _contractService.CreateContractAsync(dto));
        }

        [Fact]
        public async Task GetAllContractsAsync_ShouldReturnContracts_WhenContractsExist()
        {
            var contracts = new List<ContractDto>
            {
                new ContractDto
                {
                    ProductionFacilityName = "Facility A",
                    EquipmentTypeName = "Type A",
                    EquipmentQuantity = 10
                },
                new ContractDto
                {
                    ProductionFacilityName = "Facility B",
                    EquipmentTypeName = "Type B",
                    EquipmentQuantity = 5
                }
            };

            _contractRepositoryMock.Setup(r => r.GetAllContractsAsync()).ReturnsAsync(contracts);

            var result = await _contractService.GetAllContractsAsync();

            result.Should().BeEquivalentTo(contracts);
        }
    }
}
