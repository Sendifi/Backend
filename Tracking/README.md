# Tracking API (módulo)

Endpoints:
- GET    /trackingEvents
- GET    /trackingEvents/:id
- GET    /trackingEvents?shipmentId=XXX
- POST   /trackingEvents
- PATCH  /trackingEvents/:id
- DELETE /trackingEvents/:id

Estados válidos: REGISTERED, IN_TRANSIT, DELIVERED, CANCELLED  
Fechas: ISO 8601 (UTC). Respuestas JSON camelCase.

### DI & arranque (ejemplo)
En `Program.cs` o donde registres servicios:
```csharp
builder.Services.AddSingleton<Tracking.Domain.Repositories.ITrackingEventRepository,
    Tracking.Application.Internal.InMemory.InMemoryTrackingEventRepository>();
builder.Services.AddScoped<Tracking.Application.Internal.CommandServices.TrackingEventCommandService>();
builder.Services.AddScoped<Tracking.Application.Internal.QueryServices.TrackingEventQueryService>();
builder.Services.AddControllers(); // y CORS global si aplica
