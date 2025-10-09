# Bank API Documentation

## Endpoints disponibles

### BANK ENDPOINTS

#### 1. Crear entrada en banco
**POST** `/api/v1/bank`

**Request Body:**
```json
{
  "idAccount": 1,
  "idEds": 1,
  "idCourt": 123,
  "moviment": "CORTE",
  "ammount": 150000.50,
  "note": "Corte #123 - Banco: Banco de Bogotá - Cuenta: 001-123456789 - Medios de pago bancarios: Datafono: $100,000, Nequi: $50,000"
}
```

#### 2. Obtener todas las entradas bancarias
**GET** `/api/v1/bank?idAccount={accountId}&idEds={edsId}`

**Query Parameters:**
- `idAccount` (opcional): Filtrar por ID de cuenta
- `idEds` (opcional): Filtrar por ID de EDS

**Response:**
```json
{
  "statusCode": 200,
  "success": true,
  "message": null,
  "data": [
    {
      "idBank": 1,
      "idAccount": 1,
      "idEds": 1,
      "idCourt": 123,
      "moviment": "CORTE",
      "note": "Corte #123 - Medios de pago bancarios...",
      "ammount": 150000.50,
      "balance": 650000.50,
      "createdAt": "2024-01-01T10:00:00Z"
    }
  ]
}
```

#### 3. Obtener entrada por ID
**GET** `/api/v1/bank/{id}`

#### 4. Obtener último registro y balance por cuenta
**GET** `/api/v1/bank/balance/{accountId}`

#### 5. Obtener saldo actual por cuenta
**GET** `/api/v1/bank/current-balance/{accountId}`

**Response:**
```json
{
  "statusCode": 200,
  "success": true,
  "message": null,
  "data": {
    "balance": 750000.00,
    "accountId": 1
  }
}
```

### ACCOUNT ENDPOINTS

#### 1. Crear cuenta bancaria
**POST** `/api/v1/account`

**Request Body:**
```json
{
  "accountType": "Corriente",
  "bank": "Banco de Bogotá",
  "account": "001-123456789",
  "holder": "EDS Los Pinos S.A.S"
}
```

**Response:**
```json
{
  "statusCode": 201,
  "success": true,
  "message": null,
  "data": {
    "idAccount": 1,
    "accountType": "Corriente",
    "bank": "Banco de Bogotá",
    "account": "001-123456789",
    "holder": "EDS Los Pinos S.A.S",
    "createdAt": "2024-01-01T00:00:00Z"
  }
}
```

#### 2. Obtener todas las cuentas
**GET** `/api/v1/account`

**Response:**
```json
{
  "statusCode": 200,
  "success": true,
  "message": null,
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

#### 3. Obtener cuenta por ID
**GET** `/api/v1/account/{id}`

**Response:**
```json
{
  "statusCode": 200,
  "success": true,
  "message": null,
  "data": {
    "idAccount": 1,
    "accountType": "Corriente",
    "bank": "Banco de Bogotá",
    "account": "001-123456789",
    "holder": "EDS Los Pinos S.A.S",
    "createdAt": "2024-01-01T00:00:00Z"
  }
}
```

## Arquitectura Implementada

### **Patrón MediatR/CQRS:**
- ? **Commands**: Para operaciones de escritura (Create)
- ? **Queries**: Para operaciones de lectura (GetAll, GetById)
- ? **Handlers**: Separación clara de responsabilidades
- ? **DTOs**: Respuestas tipadas y consistentes

### **Controladores Refactorizados:**
- ? **BankController**: CRUD completo con MediatR
- ? **AccountController**: Refactorizado para usar solo MediatR
- ? **Manejo de errores consistente**: ValidationProblemDetails y Problem Details
- ? **Respuestas estandarizadas**: ResponseApiService

## Estructura de las Tablas

### Tabla Bank
| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id_bank` | int PK | ID único del registro bancario |
| `id_account` | int FK | ID de la cuenta bancaria |
| `id_eds` | int FK | ID de la EDS |
| `id_court` | int FK NULL | ID del corte (opcional) |
| `moviment` | varchar(100) | Tipo de movimiento |
| `note` | varchar(500) | Nota descriptiva |
| `ammount` | double | Monto de la transacción |
| `balance` | double | **Saldo acumulado** |
| Campos de auditoría | | createdBy, createdAt, updatedBy, updatedAt |

### Tabla Account
| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id_account` | int PK | ID único de la cuenta |
| `account_type` | varchar(100) | Tipo de cuenta |
| `bank` | varchar(100) | Nombre del banco |
| `account` | varchar(100) UNIQUE | Número de cuenta |
| `holder` | varchar(100) | Titular de la cuenta |
| Campos de auditoría | | createdBy, createdAt, updatedBy, updatedAt |

## Tipos de Movimiento Válidos

- **CORTE**: Entrada de dinero por corte de turno
- **RETIRO**: Salida de dinero de la cuenta
- **DEPOSITO**: Entrada manual de dinero

## Funcionalidades Implementadas

### ? CRUD Completo Bank
- ? **CREATE**: Crear registros bancarios
- ? **READ**: Obtener por ID, obtener todos (con filtros)
- ? **Endpoint especial**: Saldo actual (`/current-balance/{accountId}`)
- ? **Endpoint especial**: Último registro (`/balance/{accountId}`)

### ? CRUD Completo Account
- ? **CREATE**: Crear cuentas bancarias (MediatR)
- ? **READ**: Obtener por ID, obtener todos (MediatR)
- ? **Patrón consistente**: Refactorizado con MediatR completo
- ? **UPDATE/DELETE**: No implementados (por seguridad)

### ? Integración Automática
- ? **Detección automática**: Medios de pago bancarios en cortes
- ? **Registro automático**: Creación en tabla bank al hacer cortes
- ? **WhatsApp**: Sección bancaria con información completa

## Medios de Pago Bancarios Detectados

El sistema detecta automáticamente estos medios de pago:
- **Datafono**
- **Nequi**
- **Cod_QR**
- **Transferencia**
- **Bre-B**

## Diferencia entre Endpoints de Balance

### `/balance/{accountId}` 
- Devuelve el **último registro completo** con toda la información
- Útil para auditoría y ver detalles del último movimiento

### `/current-balance/{accountId}`
- Devuelve **solo el saldo actual** (número)
- Más rápido y directo
- Ideal para consultas de saldo

## Validaciones

### BankDtoCreateRequest
- `idAccount`: Debe ser mayor que 0
- `idEds`: Debe ser mayor que 0
- `idCourt`: Opcional, puede ser NULL
- `moviment`: Requerido, debe ser uno de: CORTE, RETIRO, DEPOSITO
- `ammount`: Debe ser mayor que 0
- `note`: Requerida, máximo 500 caracteres

### AccountCreateDto
- `accountType`: Requerido, máximo 100 caracteres
- `bank`: Requerido, máximo 100 caracteres
- `account`: Requerido, máximo 100 caracteres, **debe ser único**
- `holder`: Requerido, máximo 100 caracteres

## Reglas de Negocio

1. **Balance automático**: Se calcula automáticamente basado en el último registro
2. **Cuenta única**: El número de cuenta debe ser único en el sistema
3. **No retiros mayores al saldo**: El sistema valida que no se pueda retirar más de lo disponible
4. **Cada cuenta mantiene su propio balance**: Balances independientes por cuenta
5. **Relaciones FK**: Los registros están relacionados con EDS y opcionalmente con cortes

## Seguridad

Todos los endpoints requieren autenticación JWT y están protegidos con la política `AdminOrIslander`.