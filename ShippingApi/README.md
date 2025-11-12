# Sendifi - Shipping API

API RESTful para la gestión de envíos desarrollada en C# con ASP.NET Core 9.0 Minimal API.

## 🚀 Estado del Proyecto

✅ **API completamente funcional y lista para desarrollo**

- Todos los endpoints implementados y probados
- Validaciones de datos configuradas
- CORS habilitado para consumo desde el frontend
- Manejo de errores con respuestas JSON
- Datos de ejemplo (seed data) incluidos

## 📋 Endpoints Implementados

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/shipments` | Lista todos los envíos (con filtros opcionales) |
| GET | `/shipments/:id` | Obtiene un envío específico por ID |
| POST | `/shipments` | Crea un nuevo envío |
| PATCH | `/shipments/:id` | Actualiza un envío existente |
| DELETE | `/shipments/:id` | Elimina un envío |

### Filtros Disponibles (Query Parameters)

- `status` - Filtrar por estado: PENDING, IN_TRANSIT, DELIVERED, CANCELLED
- `trackingCode` - Buscar por código de tracking exacto
- `deliveryPersonId` - Filtrar por ID del repartidor

## 🔧 Tecnologías Utilizadas

- **Framework**: ASP.NET Core 9.0
- **Patrón**: Minimal API
- **Almacenamiento**: En memoria (List<Shipment>)
- **Puerto**: 5000 (http://0.0.0.0:5000)
- **JSON**: camelCase con enums en SNAKE_CASE_UPPER

## 📦 Estructura del Proyecto

```
ShippingApi/
├── Shipment/
│   ├── Models/              # Modelos de datos
│   │   ├── Address.cs
│   │   ├── Person.cs
│   │   ├── Shipment.cs
│   │   ├── ShipmentStatus.cs
│   │   ├── CreateShipmentRequest.cs
│   │   └── UpdateShipmentRequest.cs
│   └── Services/            # Lógica de negocio
│       └── ShipmentService.cs
├── Shared/                  # Utilidades compartidas
│   ├── ErrorResponse.cs
│   └── SuccessResponse.cs
├── Program.cs               # Configuración principal
└── ShippingApi.csproj
```

## 🏃 Cómo Ejecutar

### Requisitos Previos
- .NET 9.0 SDK instalado

### Ejecución Local

```bash
cd ShippingApi
dotnet run
```

La API estará disponible en: `http://localhost:5000`

## 🧪 Ejemplos de Uso

### Listar todos los envíos
```bash
curl http://localhost:5000/shipments
```

### Obtener un envío por ID
```bash
curl http://localhost:5000/shipments/1
```

### Filtrar por estado
```bash
curl "http://localhost:5000/shipments?status=PENDING"
```

### Crear un nuevo envío
```bash
curl -X POST http://localhost:5000/shipments \
  -H "Content-Type: application/json" \
  -d '{
    "sender": {
      "name": "Juan Pérez",
      "email": "juan@email.com",
      "phone": "+1234567890"
    },
    "recipient": {
      "name": "María García",
      "email": "maria@email.com",
      "phone": "+0987654321"
    },
    "originAddress": {
      "street": "Calle 123",
      "city": "Bogotá",
      "state": "Cundinamarca",
      "zipCode": "110111",
      "country": "Colombia"
    },
    "destinationAddress": {
      "street": "Avenida 456",
      "city": "Medellín",
      "state": "Antioquia",
      "zipCode": "050001",
      "country": "Colombia"
    },
    "weight": 2.5,
    "status": "PENDING",
    "courierId": 1
  }'
```

### Actualizar un envío
```bash
curl -X PATCH http://localhost:5000/shipments/1 \
  -H "Content-Type: application/json" \
  -d '{
    "status": "IN_TRANSIT",
    "estimatedDelivery": "2024-01-20"
  }'
```

### Eliminar un envío
```bash
curl -X DELETE http://localhost:5000/shipments/1
```

## 📊 Códigos de Estado HTTP

- `200 OK` - GET y PATCH exitosos
- `201 Created` - POST exitoso
- `400 Bad Request` - Datos inválidos o JSON malformado
- `404 Not Found` - Recurso no encontrado
- `500 Internal Server Error` - Error del servidor

## ✅ Validaciones Implementadas

- `weight` debe ser un número positivo
- `sender.name`, `sender.email`, `sender.phone` son obligatorios
- `recipient.name`, `recipient.email`, `recipient.phone` son obligatorios
- `originAddress.street` y `originAddress.city` son obligatorios
- `destinationAddress.street` y `destinationAddress.city` son obligatorios
- `status` debe ser: PENDING, IN_TRANSIT, DELIVERED o CANCELLED
- `trackingCode` se genera automáticamente (formato: SFY + 9 dígitos)

## 🌐 Configuración CORS

La API está configurada para permitir peticiones desde cualquier origen:
- `Access-Control-Allow-Origin: *`
- `Access-Control-Allow-Methods: GET, POST, PATCH, DELETE, OPTIONS`
- `Access-Control-Allow-Headers: Content-Type, Authorization`

## 📝 Formato de Datos

### Modelo Shipment Completo

```json
{
  "id": "1",
  "trackingCode": "SFY123456789",
  "sender": {
    "name": "Juan Pérez",
    "email": "juan@email.com",
    "phone": "+1234567890"
  },
  "recipient": {
    "name": "María García",
    "email": "maria@email.com",
    "phone": "+0987654321"
  },
  "originAddress": {
    "street": "Calle 123",
    "city": "Bogotá",
    "state": "Cundinamarca",
    "zipCode": "110111",
    "country": "Colombia"
  },
  "destinationAddress": {
    "street": "Avenida 456",
    "city": "Medellín",
    "state": "Antioquia",
    "zipCode": "050001",
    "country": "Colombia"
  },
  "weight": 2.5,
  "cost": 15000,
  "status": "PENDING",
  "courierId": 1,
  "deliveryPersonId": "e07a",
  "estimatedDelivery": "2024-01-15",
  "createdAt": "2024-01-10T10:00:00Z",
  "updatedAt": "2024-01-10T10:00:00Z"
}
```

### Estados Válidos

- `PENDING` - Pendiente
- `IN_TRANSIT` - En tránsito
- `DELIVERED` - Entregado
- `CANCELLED` - Cancelado

## 🔮 Mejoras Futuras

1. Integrar base de datos PostgreSQL para persistencia
2. Implementar autenticación y autorización JWT
3. Agregar cálculo automático de costos
4. Implementar cálculo de fecha estimada de entrega
5. Agregar historial de cambios y auditoría
6. Implementar paginación para grandes conjuntos de datos
7. Agregar Swagger/OpenAPI documentation
8. Implementar rate limiting
9. Agregar logging estructurado
10. Configurar deployment automático

## 👥 Equipo

- **Desarrollador**: Juan Diego
- **API**: Shipping API
- **Proyecto**: Sendifi - Plataforma de Gestión de Envíos

## 📄 Licencia

Este proyecto es parte del sistema Sendifi y sigue las políticas del equipo de desarrollo.
