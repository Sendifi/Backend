using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using WebApplication2.Courier.Domain.Services;
using WebApplication2.Courier.Domain.Model.Queries;
using WebApplication2.Courier.Interfaces.REST.Resources;
using WebApplication2.Courier.Interfaces.REST.Transform;

namespace WebApplication2.Courier.Interfaces.REST;

[ApiController]
[Route(template: "couriers")] // EXACTO según la asignación
[Produces(MediaTypeNames.Application.Json)]
[Tags("Couriers")]
public class CouriersController(
    ICourierCommandService cmd,
    ICourierQueryService qry
) : ControllerBase
{
    // GET /couriers
    [HttpGet]
    [SwaggerOperation(Summary = "List couriers")]
    [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(IEnumerable<CourierResource>))]
    public async Task<ActionResult> GetAll()
    {
        var list = await qry.Handle(new GetAllCouriersQuery());
        return Ok(list.Select(CourierResourceFromEntityAssembler.ToResourceFromEntity));
    }

    // GET /couriers/{id}
    [HttpGet(template: "{id:int}")]
    [SwaggerOperation(Summary = "Get courier by id")]
    [SwaggerResponse(StatusCodes.Status200OK, "Found", typeof(CourierResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
    public async Task<ActionResult> GetById(int id)
    {
        var found = await qry.Handle(new GetCourierByIdQuery(id));
        if (found is null) return NotFound(new { message = "Courier not found" });
        return Ok(CourierResourceFromEntityAssembler.ToResourceFromEntity(found));
    }

    // POST /couriers
    [HttpPost]
    [SwaggerOperation(Summary = "Create courier")]
    [SwaggerResponse(StatusCodes.Status201Created, "Created", typeof(CourierResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error")]
    public async Task<ActionResult> Create([FromBody] CreateCourierResource resource)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var command = CreateCourierCommandFromResourceAssembler.ToCommandFromResource(resource);
        var created = await cmd.Handle(command);

        var createdResource = CourierResourceFromEntityAssembler.ToResourceFromEntity(created);
        return CreatedAtAction(nameof(GetById), new { id = createdResource.Id }, createdResource);
    }

    // PATCH /couriers/{id}
    [HttpPatch(template: "{id:int}")]
    [SwaggerOperation(Summary = "Update courier")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updated", typeof(CourierResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCourierResource resource)
    {
        var updated = await cmd.Update(
            id,
            resource.Name,            // string? 
            resource.CostPerKg,       // decimal?
            resource.EstimatedDays,   // int?
            resource.IsActive         // bool?
        );

        if (updated is null) return NotFound(new { message = "Courier not found" });
        return Ok(CourierResourceFromEntityAssembler.ToResourceFromEntity(updated));
    }

    // DELETE /couriers/{id}
    [HttpDelete(template: "{id:int}")]
    [SwaggerOperation(Summary = "Delete courier")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
    public async Task<ActionResult> Delete(int id)
    {
        var ok = await cmd.Delete(id);
        if (!ok) return NotFound(new { message = "Courier not found" });
        return NoContent();
    }
}
