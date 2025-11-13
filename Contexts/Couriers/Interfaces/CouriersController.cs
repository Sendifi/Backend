using backSendify.Couriers.Application.Abstractions;
using backSendify.Couriers.Application.Couriers.Dtos;
using backSendify.Couriers.Application.Couriers.Requests;
using Microsoft.AspNetCore.Mvc;

namespace backSendify.Couriers.Interfaces;

[ApiController]
[Route("couriers")]
public class CouriersController : ControllerBase
{
    private readonly ICourierService _courierService;

    public CouriersController(ICourierService courierService)
    {
        _courierService = courierService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CourierDto>>> GetCouriers(CancellationToken cancellationToken)
    {
        var couriers = await _courierService.GetAsync(cancellationToken);
        return Ok(couriers);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourierDto>> GetCourier(Guid id, CancellationToken cancellationToken)
    {
        var courier = await _courierService.GetByIdAsync(id, cancellationToken);
        return courier is null
            ? NotFound(new { message = "Courier not found" })
            : Ok(courier);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CourierDto>> CreateCourier([FromBody] CourierCreateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var courier = await _courierService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetCourier), new { id = courier.Id }, courier);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourierDto>> UpdateCourier(Guid id, [FromBody] CourierUpdateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var courier = await _courierService.UpdateAsync(id, request, cancellationToken);
            return courier is null
                ? NotFound(new { message = "Courier not found" })
                : Ok(courier);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCourier(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _courierService.DeleteAsync(id, cancellationToken);
        return deleted
            ? Ok(new { success = true })
            : NotFound(new { message = "Courier not found" });
    }
}
