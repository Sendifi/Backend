using WebApplication2.Courier.Domain.Model.Commands;
using WebApplication2.Courier.Domain.Model.Aggregates;
using WebApplication2.Courier.Interfaces.REST.Resources;

namespace WebApplication2.Courier.Interfaces.REST.Transform;

public static class CreateCourierCommandFromResourceAssembler
{
    public static CreateCourierCommand ToCommandFromResource(CreateCourierResource r) =>
        new(r.Name, r.CostPerKg, r.EstimatedDays, r.IsActive);
}