using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tracking.Application.Internal.CommandServices;
using Tracking.Application.Internal.QueryServices;
using Tracking.Domain.Commands;
using Tracking.Domain.Queries;
using Tracking.Interfaces.REST.Resources;
using Tracking.Interfaces.REST.Transform;

namespace Tracking.Interfaces.REST.Controllers
{
    [ApiController]
    [Route("trackingEvents")]
    [Produces(MediaTypeNames.Application.Json)]
    public class TrackingEventsController : ControllerBase
    {
        private readonly TrackingEventCommandService _commands;
        private readonly TrackingEventQueryService _queries;

        public TrackingEventsController(
            TrackingEventCommandService commands,
            TrackingEventQueryService queries)
        {
            _commands = commands;
            _queries = queries;
        }

        // GET /trackingEvents?shipmentId=XXX
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? shipmentId)
        {
            var data = await _queries.Handle(new GetTrackingEventsQuery(shipmentId));
            var outp = data.Select(TrackingEventResourceFromEntityAssembler.ToResource);
            return Ok(outp);
        }

        // GET /trackingEvents/:id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _queries.Handle(new GetTrackingEventByIdQuery(id));
            if (result is null) return NotFound(new { message = "TrackingEvent not found" });
            return Ok(TrackingEventResourceFromEntityAssembler.ToResource(result));
        }

        // POST /trackingEvents
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTrackingEventResource body)
        {
            if (body is null || string.IsNullOrWhiteSpace(body.ShipmentId)
                || string.IsNullOrWhiteSpace(body.Status)
                || string.IsNullOrWhiteSpace(body.Description))
                return BadRequest(new { message = "Invalid input data" });

            try
            {
                var created = await _commands.Handle(
                    CreateTrackingEventCommandFromResourceAssembler.ToCommand(body));
                var resource = TrackingEventResourceFromEntityAssembler.ToResource(created);
                return CreatedAtAction(nameof(GetById), new { id = resource.Id }, resource);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PATCH /trackingEvents/:id
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTrackingEventResource body)
        {
            try
            {
                var updated = await _commands.Handle(
                    UpdateTrackingEventCommandFromResourceAssembler.ToCommand(id, body));
                if (updated is null) return NotFound(new { message = "TrackingEvent not found" });
                return Ok(TrackingEventResourceFromEntityAssembler.ToResource(updated));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /trackingEvents/:id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ok = await _commands.Handle(new DeleteTrackingEventCommand(id));
            if (!ok) return NotFound(new { message = "TrackingEvent not found" });
            return Ok(new { success = true });
        }
    }
}
