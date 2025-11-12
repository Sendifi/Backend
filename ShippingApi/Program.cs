using ShippingApi.Shipment.Models;
using ShippingApi.Shipment.Services;
using ShippingApi.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ShipmentService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        
        var exceptionHandlerFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;
        
        ErrorResponse errorResponse;
        
        if (exception is Microsoft.AspNetCore.Http.BadHttpRequestException || 
            exception is System.Text.Json.JsonException)
        {
            context.Response.StatusCode = 400;
            errorResponse = new ErrorResponse 
            { 
                Message = "Invalid input data",
                Errors = new List<string> { "Request body contains invalid JSON or missing required fields" }
            };
        }
        else
        {
            context.Response.StatusCode = 500;
            errorResponse = new ErrorResponse 
            { 
                Message = "Internal server error"
            };
        }
        
        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});

app.UseCors();

app.MapGet("/shipments", (ShipmentService service, string? status, string? trackingCode, string? deliveryPersonId) =>
{
    try
    {
        var shipments = service.GetAllShipments(status, trackingCode, deliveryPersonId);
        return Results.Ok(shipments);
    }
    catch (Exception ex)
    {
        return Results.Json(
            new ErrorResponse { Message = "Internal server error" },
            statusCode: 500
        );
    }
});

app.MapGet("/shipments/{id}", (string id, ShipmentService service) =>
{
    try
    {
        var shipment = service.GetShipmentById(id);
        if (shipment == null)
        {
            return Results.Json(
                new ErrorResponse { Message = "Shipment not found" },
                statusCode: 404
            );
        }
        return Results.Ok(shipment);
    }
    catch (Exception ex)
    {
        return Results.Json(
            new ErrorResponse { Message = "Internal server error" },
            statusCode: 500
        );
    }
});

app.MapPost("/shipments", (CreateShipmentRequest request, ShipmentService service) =>
{
    try
    {
        var (isValid, errors) = service.ValidateCreateRequest(request);
        if (!isValid)
        {
            return Results.Json(
                new ErrorResponse 
                { 
                    Message = "Invalid input data",
                    Errors = errors
                },
                statusCode: 400
            );
        }

        var shipment = service.CreateShipment(request);
        return Results.Created($"/shipments/{shipment.Id}", shipment);
    }
    catch (Exception ex)
    {
        return Results.Json(
            new ErrorResponse { Message = "Internal server error" },
            statusCode: 500
        );
    }
});

app.MapPatch("/shipments/{id}", (string id, UpdateShipmentRequest request, ShipmentService service) =>
{
    try
    {
        var shipment = service.UpdateShipment(id, request);
        if (shipment == null)
        {
            return Results.Json(
                new ErrorResponse { Message = "Shipment not found" },
                statusCode: 404
            );
        }
        return Results.Ok(shipment);
    }
    catch (Exception ex)
    {
        return Results.Json(
            new ErrorResponse { Message = "Internal server error" },
            statusCode: 500
        );
    }
});

app.MapDelete("/shipments/{id}", (string id, ShipmentService service) =>
{
    try
    {
        var deleted = service.DeleteShipment(id);
        if (!deleted)
        {
            return Results.Json(
                new ErrorResponse { Message = "Shipment not found" },
                statusCode: 404
            );
        }
        return Results.Ok(new SuccessResponse { Success = true });
    }
    catch (Exception ex)
    {
        return Results.Json(
            new ErrorResponse { Message = "Internal server error" },
            statusCode: 500
        );
    }
});

app.Run();
