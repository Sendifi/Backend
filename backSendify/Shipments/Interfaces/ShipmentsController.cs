using backSendify.Shipments.Application.Abstractions;
using backSendify.Shipments.Application.Shipments.Dtos;
using backSendify.Shipments.Application.Shipments.Requests;
using backSendify.Shipments.Domain.Shipments.Enums;
using Microsoft.AspNetCore.Mvc;

namespace backSendify.Shipments.Interfaces;

[ApiController]
[Route("shipments")]
public class ShipmentsController : ControllerBase
{
    private readonly IShipmentService _shipmentService;

    public ShipmentsController(IShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ShipmentDto>>> GetShipments([
        FromQuery] ShipmentStatus? status,
        [FromQuery] string? trackingCode,
        [FromQuery] Guid? deliveryPersonId,
        CancellationToken cancellationToken)
    {
        var query = new ShipmentQueryParameters
        {
            Status = status,
            TrackingCode = trackingCode,
            DeliveryPersonId = deliveryPersonId
        };
        var shipments = await _shipmentService.GetShipmentsAsync(query, cancellationToken);
        return Ok(shipments);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShipmentDto>> GetShipment(Guid id, CancellationToken cancellationToken)
    {
        var shipment = await _shipmentService.GetByIdAsync(id, cancellationToken);
        if (shipment == null)
        {
            return NotFound(new { message = "Shipment not found" });
        }

        return Ok(shipment);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ShipmentDto>> CreateShipment([FromBody] ShipmentCreateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var shipment = await _shipmentService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetShipment), new { id = shipment.Id }, shipment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShipmentDto>> UpdateShipment(Guid id, [FromBody] ShipmentUpdateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var shipment = await _shipmentService.UpdateAsync(id, request, cancellationToken);
            if (shipment == null)
            {
                return NotFound(new { message = "Shipment not found" });
            }

            return Ok(shipment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteShipment(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _shipmentService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound(new { message = "Shipment not found" });
        }

        return Ok(new { success = true });
    }
}
