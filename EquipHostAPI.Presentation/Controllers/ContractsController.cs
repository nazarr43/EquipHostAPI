using EquipHostAPI.Application.DTOs;
using EquipHostAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace EquipHostAPI.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] CreateContractDto dto)
    {
        var contractId = await _contractService.CreateContractAsync(dto);
        return CreatedAtAction(nameof(GetAllContracts), new { id = contractId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContracts()
    {
        var contracts = await _contractService.GetAllContractsAsync();
        return Ok(contracts);
    }
}

