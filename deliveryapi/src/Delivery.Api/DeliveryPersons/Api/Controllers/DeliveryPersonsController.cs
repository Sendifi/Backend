using Delivery.Api.DeliveryPersons.Api.Dtos;
using Delivery.Api.DeliveryPersons.Api.Mappers;
using Delivery.Api.DeliveryPersons.Domain.Commands;
using Delivery.Api.DeliveryPersons.Domain.Queries;
using Delivery.Api.DeliveryPersons.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Api.DeliveryPersons.Api.Controllers;

[ApiController]
[Route("deliveryPersons")] // 👈 EXACTAMENTE como lo pide el enunciado
public class DeliveryPersonsController : ControllerBase
{
    private readonly IDeliveryPersonCommandService _commandService;
    private readonly IDeliveryPersonQueryService _queryService;

    public DeliveryPersonsController(
        IDeliveryPersonCommandService commandService,
        IDeliveryPersonQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    // GET /deliveryPersons?code=DEL001
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeliveryPersonResource>>> GetAll([FromQuery] string? code)
    {
        if (!string.IsNullOrWhiteSpace(code))
        {
            var person = await _queryService.Handle(new GetDeliveryPersonByCodeQuery(code));

            if (person is null) return NotFound();

            return Ok(new[] { person.ToResource() });
        }

        var query = new GetAllDeliveryPersonsQuery();
        var persons = await _queryService.Handle(query);

        return Ok(persons.Select(p => p.ToResource()));
    }

    // GET /deliveryPersons/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<DeliveryPersonResource>> GetById(string id)
    {
        var person = await _queryService.Handle(new GetDeliveryPersonByIdQuery(id));

        if (person is null) return NotFound();

        return Ok(person.ToResource());
    }

    // POST /deliveryPersons
    [HttpPost]
    public async Task<ActionResult<DeliveryPersonResource>> Create(
        [FromBody] CreateDeliveryPersonResource resource)
    {
        var command = resource.ToCreateCommand();
        var created = await _commandService.Handle(command);

        var result = created.ToResource();

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PATCH /deliveryPersons/{id}
    [HttpPatch("{id}")]
    public async Task<ActionResult<DeliveryPersonResource>> Update(
        string id,
        [FromBody] UpdateDeliveryPersonResource resource)
    {
        var command = resource.ToUpdateCommand(id);
        var updated = await _commandService.Handle(command);

        if (updated is null) return NotFound();

        return Ok(updated.ToResource());
    }

    // DELETE /deliveryPersons/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _commandService.Handle(new DeleteDeliveryPersonCommand(id));

        if (!success) return NotFound();

        return Ok(new { success = true });
    }
}
