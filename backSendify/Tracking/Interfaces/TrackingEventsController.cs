using backSendify.Tracking.Application.Abstractions;
using backSendify.Tracking.Application.Tracking.Dtos;
using backSendify.Tracking.Application.Tracking.Requests;
using Microsoft.AspNetCore.Mvc;

namespace backSendify.Tracking.Interfaces;

[ApiController]
[Route("trackingEvents")]
public class TrackingEventsController : ControllerBase
{
    private readonly ITrackingEventService _trackingEventService;

    public TrackingEventsController(ITrackingEventService trackingEventService)
    {
        _trackingEventService = trackingEventService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TrackingEventDto>>> GetTrackingEvents([
        FromQuery] Guid? shipmentId,
        CancellationToken cancellationToken)
    {
        var query = new TrackingEventQueryParameters { ShipmentId = shipmentId };
        var events = await _trackingEventService.GetAsync(query, cancellationToken);
        return Ok(events);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TrackingEventDto>> GetTrackingEvent(Guid id, CancellationToken cancellationToken)
    {
        var trackingEvent = await _trackingEventService.GetByIdAsync(id, cancellationToken);
        return trackingEvent is null
            ? NotFound(new { message = "Tracking event not found" })
            : Ok(trackingEvent);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<TrackingEventDto>> CreateTrackingEvent([FromBody] TrackingEventCreateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var trackingEvent = await _trackingEventService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetTrackingEvent), new { id = trackingEvent.Id }, trackingEvent);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TrackingEventDto>> UpdateTrackingEvent(Guid id, [FromBody] TrackingEventUpdateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var trackingEvent = await _trackingEventService.UpdateAsync(id, request, cancellationToken);
            return trackingEvent is null
                ? NotFound(new { message = "Tracking event not found" })
                : Ok(trackingEvent);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteTrackingEvent(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _trackingEventService.DeleteAsync(id, cancellationToken);
        return deleted
            ? Ok(new { success = true })
            : NotFound(new { message = "Tracking event not found" });
    }
}
