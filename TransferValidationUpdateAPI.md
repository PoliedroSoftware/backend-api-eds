# Transfer Validation API Update Method

## Overview
The update method for the Transfer Validation API allows administrators to update existing transfer validation records in the system. The ID is passed as a query string parameter, while the data to update is sent in the request body.

## Endpoint
```
PUT /api/v1/transfer-validation?id={id}
```

## Architecture Components Created

### 1. **Command and DTOs**
- `UpdateTransferValidationCommand.cs` - MediatR command (receives ID and request separately)
- `UpdateTransferValidationRequestDto.cs` - Request DTO for the API (without ID)
- `UpdateTransferValidationValidator.cs` - FluentValidation validator

### 2. **Command Handler**
- `UpdateTransferValidationCommandHandler.cs` - Handles the update logic with:
  - ID validation (separate from request DTO)
  - Request validation using FluentValidation
  - Domain service integration
  - Comprehensive logging
  - Exception handling

### 3. **Domain Services**
- `ITransferValidationService.UpdateAsync()` - Domain service interface method
- `TransferValidationService.UpdateAsync()` - Domain service implementation

### 4. **Repositories**
- `ITransferValidationRepositoryGetById.cs` - Interface for getting entity by ID
- `ITransferValidationRepositoryUpdate.cs` - Interface for updating entities
- `TransferValidationRepositoryGetById.cs` - Implementation for getting by ID
- `TransferValidationRepositoryUpdate.cs` - Implementation for updating

### 5. **Controller Method**
- `UpdateTransferValidationController.Update()` - HTTP PUT endpoint with query string ID

## Request Structure
**URL**: `PUT /api/v1/transfer-validation?id=1`

**Request Body**:
```json
{
  "customerName": "Juan Pérez",
  "transactionAmount": 150000.50,
  "transactionDate": "2024-01-15",
  "transactionTime": "14:30:00",
  "status": "CONFIRMADA",
  "confirmedBy": "admin@company.com"
}
```

## Request Parameters
### Query String Parameters
- **id** (int, required): ID of the validation record to update

### Request Body Properties
- **customerName** (string, required): Customer name (max 150 chars)
- **transactionAmount** (double, required): Transaction amount (> 0, <= 999999999.99)
- **transactionDate** (DateOnly, required): Transaction date (not future)
- **transactionTime** (TimeOnly, required): Transaction time
- **status** (string, required): Status (PENDIENTE, CONFIRMADA, RECHAZADA)
- **confirmedBy** (string, optional): Required when status is CONFIRMADA

## Response Codes
- **200 OK**: Update successful
- **400 Bad Request**: Invalid ID or request parameters, validation errors
- **401 Unauthorized**: Missing or invalid authentication
- **404 Not Found**: Transfer validation record not found
- **500 Internal Server Error**: Server error

## Validation Rules
### ID Validation (Query String)
- ID must be greater than 0

### Request Body Validation
- Customer name is required and max 150 characters
- Transaction amount must be > 0 and <= 999,999,999.99
- Transaction date cannot be in the future
- Status must be one of: PENDIENTE, CONFIRMADA, RECHAZADA
- If status is CONFIRMADA, confirmedBy is required
- ConfirmedBy max 255 characters

## Architecture Changes for Query String ID
The implementation was modified to separate the ID from the request body:

1. **Controller** receives ID from `[FromQuery]` and data from `[FromBody]`
2. **Command** takes ID as a separate parameter: `UpdateTransferValidationCommand(int Id, UpdateTransferValidationRequestDto Request)`
3. **Command Handler** validates ID separately and uses it for domain service call
4. **Request DTO** no longer contains the `IdTransferValidation` property
5. **Validator** only validates the request body properties, not the ID

## Sample API Call
```bash
curl -X PUT "https://api.example.com/api/v1/transfer-validation?id=123" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer {token}" \
-d '{
  "customerName": "María García",
  "transactionAmount": 250000.00,
  "transactionDate": "2024-01-20",
  "transactionTime": "09:15:00",
  "status": "CONFIRMADA",
  "confirmedBy": "supervisor@company.com"
}'
```

## Error Examples
### Invalid ID:
```
PUT /api/v1/transfer-validation?id=0
Response: 400 Bad Request
{
  "error": "El ID de la validación de transferencia debe ser mayor a cero"
}
```

### Entity Not Found:
```
PUT /api/v1/transfer-validation?id=999999
Response: 404 Not Found
{
  "error": "No se encontró la validación de transferencia con ID: 999999"
}
```

The implementation now cleanly separates the ID (passed via query string) from the update data (passed via request body), providing a more RESTful API design while maintaining all business validation and error handling capabilities.