using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Opa.Credits.Application.DTOs;
using Opa.Credits.Application.Services;

namespace Opa.Credits.Api.Controllers;

[Authorize] // Protege TODOS los endpoints de este controlador
[ApiController]
[Route("api/v1/[controller]")]
public class CreditsController : ControllerBase
{
    private readonly ICreditService _creditService;
    private readonly IValidator<CreateCreditDto> _validator;

    public CreditsController(ICreditService creditService, IValidator<CreateCreditDto> validator)
    {
        _creditService = creditService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCredit([FromBody] CreateCreditDto dto)
    {
        // 1. Validación de Entrada (DTO) usando FluentValidation
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            // Estructuramos el error según lo requerido en la prueba (PDF)
            var errorMessage = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new 
            { 
                success = false, 
                error = new { code = "VALIDATION_ERROR", message = errorMessage } 
            });
        }

        // 2. Ejecutar la lógica de negocio pura
        var result = await _creditService.CreateCreditAsync(dto);
        return CreatedAtAction(nameof(GetCredit), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginatedCredits([FromQuery] CreditFilterDto filter)
    {
        var result = await _creditService.GetPaginatedAsync(filter);
        return Ok(new { success = true, data = result });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCredit(int id)
    {
        var result = await _creditService.GetByIdAsync(id);
        return Ok(new { success = true, data = result });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCredit(int id, [FromBody] UpdateCreditDto dto)
    {
        await _creditService.UpdateAsync(id, dto);
        return Ok(new { success = true, message = "Crédito actualizado correctamente" });
    }

    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeStatusDto dto)
    {
        await _creditService.ChangeStatusAsync(id, dto);
        return Ok(new { success = true, message = "Estado actualizado correctamente" });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCredit(int id)
    {
        await _creditService.DeleteAsync(id);
        return Ok(new { success = true, message = "Crédito cancelado (borrado lógico) correctamente" });
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetDashboardSummary()
    {
        var result = await _creditService.GetDashboardSummaryAsync();
        return Ok(new { success = true, data = result });
    }
}
