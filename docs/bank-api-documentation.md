# Bank API Documentation

## Endpoints disponibles

### 1. Crear entrada en banco
**POST** `/api/v1/bank`

**Request Body:**
```json
{
  "idAccount": 1,
  "idEds": 1,
  "idCourt": 123,
  "moviment": "CORTE",
  "ammount": 150000.50,
  "note": "Corte #123 - Medios de pago bancarios: Datafono: $100,000, Nequi: $50,000"
}
```

**Response:**
```json
{
  "statusCode": 201,
  "data": {
    "idBank": 1,
    "idAccount": 1,
    "idEds": 1,
    "idCourt": 123,
    "moviment": "CORTE",
    "note": "Corte #123 - Medios de pago bancarios: Datafono: $100,000, Nequi: $50,000",
    "ammount": 150000.50,
    "balance": 150000.50,
    "createdAt": "2024-01-01T10:00:00Z"
  }
}
```

### 2. Obtener entrada por ID
**GET** `/api/v1/bank/{id}`

**Response:**
```json
{
  "statusCode": 200,
  "data": {
    "idBank": 1,
    "idAccount": 1,
    "idEds": 1,
    "idCourt": 123,
    "moviment": "CORTE",
    "note": "Corte #123 - Medios de pago bancarios: Datafono: $100,000, Nequi: $50,000",
    "ammount": 150000.50,
    "balance": 150000.50,
    "createdAt": "2024-01-01T10:00:00Z"
  }
}
```

### 3. Obtener balance por cuenta
**GET** `/api/v1/bank/balance/{accountId}`

**Response:**
```json
{
  "statusCode": 200,
  "data": {
    "idBank": 5,
    "idAccount": 1,
    "idEds": 1,
    "idCourt": 126,
    "moviment": "CORTE",
    "note": "Último movimiento",
    "ammount": 50000.00,
    "balance": 750000.00,
    "createdAt": "2024-01-01T15:30:00Z"
  }
}
```

### 4. Obtener todas las cuentas
**GET** `/api/v1/account`

**Response:**
```json
{
  "statusCode": 200,
  "data": [
    {
      "idAccount": 1,
      "accountType": "Corriente",
      "bank": "Banco de Bogotá",
      "account": "001-123456789",
      "holder": "EDS Los Pinos S.A.S",
      "createdAt": "2024-01-01T00:00:00Z"
    }
  ]
}
```

## Estructura de la Tabla Bank

La tabla `bank` contiene los siguientes campos:

- **id_bank**: ID único del registro bancario (clave primaria)
- **id_account**: ID de la cuenta bancaria (FK a tabla account)
- **id_eds**: ID de la EDS (FK a tabla eds)
- **id_court**: ID del corte (FK a tabla court, puede ser NULL)
- **moviment**: Tipo de movimiento (ej: "CORTE", "RETIRO", "DEPOSITO")
- **note**: Nota descriptiva del movimiento
- **ammount**: Monto del movimiento
- **balance**: Balance después del movimiento
- **Campos de auditoría**: createdBy, createdAt, updatedBy, updatedAt

## Estructura de la Tabla Account

La tabla `account` contiene los siguientes campos:

- **id_account**: ID único de la cuenta (clave primaria)
- **account_type**: Tipo de cuenta (ej: "Corriente", "Ahorros")
- **bank**: Nombre del banco (ej: "Banco de Bogotá")
- **account**: Número de cuenta (debe ser único)
- **holder**: Titular de la cuenta
- **Campos de auditoría**: createdBy, createdAt, updatedBy, updatedAt

## Tipos de Movimiento Válidos

- **CORTE**: Entrada de dinero por corte de turno
- **RETIRO**: Salida de dinero de la cuenta
- **DEPOSITO**: Entrada manual de dinero

## Medios de Pago Bancarios Detectados Automáticamente

El sistema detecta automáticamente estos medios de pago y crea registro en banco:

- **Datafono**
- **Nequi**
- **Cod_QR**
- **Transferencia**
- **Bre-B**

## Integración con WhatsApp

Cuando se crea un corte que incluye medios de pago bancarios, el mensaje de WhatsApp incluirá una nueva sección:

```
??????????????
?? RESUMEN BANCARIO  
??????????????
?? Total En Banco: $150,000
```

Esta sección aparece debajo del resumen de caja fuerte en el mensaje.

## Validaciones

### BankDtoCreateRequest
- `idAccount`: Debe ser mayor que 0
- `idEds`: Debe ser mayor que 0
- `idCourt`: Opcional, puede ser NULL
- `moviment`: Requerido, debe ser uno de: CORTE, RETIRO, DEPOSITO
- `ammount`: Debe ser mayor que 0
- `note`: Requerida, máximo 500 caracteres

### Reglas de Negocio
- No se puede hacer un retiro mayor al saldo actual de la cuenta
- El balance se calcula automáticamente basado en el último registro de la cuenta
- Cada cuenta mantiene su propio balance independiente
- El número de cuenta debe ser único en el sistema
- Los registros están relacionados con EDS y opcionalmente con cortes

## Seguridad

Todos los endpoints requieren autenticación JWT y están protegidos con la política `AdminOrIslander`.