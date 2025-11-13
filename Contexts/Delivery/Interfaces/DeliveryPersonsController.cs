using backSendify.Delivery.Application.Abstractions;
using backSendify.Delivery.Application.DeliveryPersons.Dtos;
using backSendify.Delivery.Application.DeliveryPersons.Requests;
using Microsoft.AspNetCore.Mvc;

namespace backSendify.Delivery.Interfaces;

[ApiController]
[Route("deliveryPersons")]
public class DeliveryPersonsController : ControllerBase
{
    private readonly IDeliveryPersonService _deliveryPersonService;

    public DeliveryPersonsController(IDeliveryPersonService deliveryPersonService)
    {
        _deliveryPersonService = deliveryPersonService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DeliveryPersonDto>>> GetDeliveryPersons([
        FromQuery] string? code,
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken)
    {
        var query = new DeliveryPersonQueryParameters
        {
            Code = code,
            IsActive = isActive
        };

        var deliveryPeople = await _deliveryPersonService.GetAsync(query, cancellationToken);
        return Ok(deliveryPeople);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeliveryPersonDto>> GetDeliveryPerson(Guid id, CancellationToken cancellationToken)
    {
        var deliveryPerson = await _deliveryPersonService.GetByIdAsync(id, cancellationToken);
        return deliveryPerson is null
            ? NotFound(new { message = "Delivery person not found" })
            : Ok(deliveryPerson);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<DeliveryPersonDto>> CreateDeliveryPerson([FromBody] DeliveryPersonCreateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var deliveryPerson = await _deliveryPersonService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetDeliveryPerson), new { id = deliveryPerson.Id }, deliveryPerson);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeliveryPersonDto>> UpdateDeliveryPerson(Guid id, [FromBody] DeliveryPersonUpdateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var deliveryPerson = await _deliveryPersonService.UpdateAsync(id, request, cancellationToken);
            return deliveryPerson is null
                ? NotFound(new { message = "Delivery person not found" })
                : Ok(deliveryPerson);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteDeliveryPerson(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _deliveryPersonService.DeleteAsync(id, cancellationToken);
        return deleted
            ? Ok(new { success = true })
            : NotFound(new { message = "Delivery person not found" });
    }
}
