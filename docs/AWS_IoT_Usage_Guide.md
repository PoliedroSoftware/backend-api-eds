# AWS IoT Core - Guía de Uso

## ?? IMPORTANTE: Configuración de Credenciales

Antes de usar este endpoint, **debes configurar las credenciales de AWS**.

?? **Ver guía completa**: [AWS_IoT_Credentials_Setup.md](AWS_IoT_Credentials_Setup.md)

**Quick Start:**
```bash
# Opción 1: Variables de ambiente
$env:AWS_ACCESS_KEY_ID="tu_access_key"
$env:AWS_SECRET_ACCESS_KEY="tu_secret_key"
$env:AWS_REGION="us-east-2"

# Opción 2: Archivo ~/.aws/credentials
[default]
aws_access_key_id = tu_access_key
aws_secret_access_key = tu_secret_key
```

---

## ?? Configuración

### appsettings.json
```json
{
  "AWS": {
    "Region": "us-east-2",
    "IoT": {
      "Endpoint": "a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com",
      "DefaultTopic": "psr/dev/psr-4g-at/cmd/"
    }
  }
}
```

## ?? Endpoint

**POST** `/api/v1/iot/publish`

### Headers
```
Authorization: Bearer {your_jwt_token}
Content-Type: application/json
```

## ?? Ejemplos de Request

### Ejemplo 1: Monitoreo de Dispensador
```json
{
  "message": {
    "input1": "dispenserId_001",
    "input2": "volume_100.5L",
    "output1": "status_active",
    "output2": "2024-01-15T10:30:00Z"
  },
  "topic": "psr/dev/psr-4g-at/cmd/"
}
```

### Ejemplo 2: Control de Bomba
```json
{
  "message": {
    "input1": "pumpId_456",
    "input2": "command_start",
    "output1": "ack_received",
    "output2": "status_running"
  },
  "topic": "psr/dev/psr-4g-at/cmd/"
}
```

### Ejemplo 3: Telemetría de Tanque
```json
{
  "message": {
    "input1": "tankId_789",
    "input2": "level_75.5%",
    "output1": "capacity_10000L",
    "output2": "alert_none"
  },
  "topic": "psr/dev/psr-4g-at/cmd/"
}
```

### Ejemplo 4: Transacción Completa
```json
{
  "message": {
    "input1": "courtId_123_dispenserId_005",
    "input2": "transaction_complete_150.5L",
    "output1": "amount_$175.50",
    "output2": "timestamp_2024-01-15T10:30:45Z"
  },
  "topic": "psr/dev/psr-4g-at/cmd/"
}
```

### Ejemplo 5: Usando Topic por Defecto (omitiendo el campo topic)
```json
{
  "message": {
    "input1": "device_test",
    "input2": "ping",
    "output1": "pong",
    "output2": "ok"
  }
}
```
> El topic se tomará de la configuración: `AWS:IoT:DefaultTopic`

## ? Respuesta Exitosa (200 OK)

```json
{
  "statusCode": 200,
  "data": {
    "success": true,
    "message": "IoT message published successfully",
    "topic": "psr/dev/psr-4g-at/cmd/"
  },
  "message": "Success",
  "errors": null
}
```

## ? Respuesta con Error (400 Bad Request)

```json
{
  "type": "https://example.com/validation-error",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "errors": {
    "Message.Input1": [
      "Input1 is required"
    ],
    "Topic": [
      "Topic must not exceed 256 characters"
    ]
  }
}
```

## ? Respuesta con Error (500 Internal Server Error)

```json
{
  "statusCode": 500,
  "data": "Failed to publish message to IoT Core",
  "message": "Internal Server Error",
  "errors": null
}
```

## ?? Validaciones

El endpoint valida:

? **Message** es requerido  
? **Input1** es requerido (máximo 255 caracteres)  
? **Input2** es requerido (máximo 255 caracteres)  
? **Output1** es requerido (máximo 255 caracteres)  
? **Output2** es requerido (máximo 255 caracteres)  
? **Topic** es opcional (máximo 256 caracteres)  

## ?? Autorización

Requiere uno de los siguientes roles:
- **Admin**
- **User** (Islander)

## ?? Estructura de Topics Recomendada

```
psr/                              # Prefijo de la empresa
  ??? dev/                        # Ambiente (dev, prod, test)
      ??? psr-4g-at/             # Dispositivo o grupo
          ??? cmd/                # Comandos
          ??? telemetry/          # Telemetría
          ??? status/             # Estados
          ??? alerts/             # Alertas
```

### Ejemplos de Topics por Caso de Uso:

| Caso de Uso | Topic | Descripción |
|-------------|-------|-------------|
| Comandos | `psr/dev/psr-4g-at/cmd/` | Envío de comandos a dispositivos |
| Telemetría | `psr/dev/psr-4g-at/telemetry/` | Datos de sensores |
| Estados | `psr/dev/psr-4g-at/status/` | Estado actual de dispositivos |
| Alertas | `psr/dev/psr-4g-at/alerts/` | Notificaciones y alertas |

## ?? Probar en Swagger

1. Ir a la URL de Swagger (generalmente `/swagger` o raíz del proyecto)
2. Buscar el endpoint **POST /api/v1/iot/publish**
3. Click en "Try it out"
4. Autenticarse con "Authorize" usando tu JWT token
5. Copiar uno de los ejemplos anteriores
6. Click en "Execute"

## ?? Troubleshooting

### Error: "No such host is known" ? SOLUCIONADO
**Este error ya NO debería ocurrir**. El servicio ahora usa el SDK oficial de AWS con autenticación correcta.

### Error: "Unable to get IAM security credentials"
**Solución**: Configura las credenciales de AWS. Ver [AWS_IoT_Credentials_Setup.md](AWS_IoT_Credentials_Setup.md)

### Error: "UnauthorizedException" o "ForbiddenException"
**Solución**: 
- Verifica que las credenciales de AWS sean correctas
- Verifica que el usuario/role tenga permisos de `iot:Publish`
- Ver guía de permisos en [AWS_IoT_Credentials_Setup.md](AWS_IoT_Credentials_Setup.md)

### Error: "Failed to initialize AWS IoT Data client"
**Solución**: 
- Verifica que el endpoint esté correcto en `appsettings.json`
- Verifica que la región sea correcta (us-east-2)
- Revisa los logs de la aplicación para más detalles

### Error: 401 Unauthorized (del API)
**Solución**: Asegúrate de incluir el token JWT en el header Authorization

### Mensaje no llega a AWS IoT
**Solución**:
- Verifica en AWS IoT Console MQTT test client
- Suscríbete al topic usando wildcard: `psr/dev/psr-4g-at/cmd/#`
- Verifica los logs de la aplicación:
  ```
  [Information] Message published successfully to topic: psr/dev/psr-4g-at/cmd/
  ```

## ?? Logs Mejorados

La aplicación ahora genera logs más detallados:

### Inicialización exitosa:
```
[Information] AWS IoT Data client initialized successfully with endpoint: a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com
```

### Publicación exitosa:
```
[Information] Attempting to publish message to IoT topic: psr/dev/psr-4g-at/cmd/
[Debug] Message payload: {"input1":"test","input2":"connection","output1":"success","output2":"ok"}
[Information] Message published successfully to topic: psr/dev/psr-4g-at/cmd/
```

### Errores específicos:
```
[Error] AWS IoT Data error publishing message to topic: psr/dev/psr-4g-at/cmd/. Error Code: UnauthorizedException, Status: 401
[Error] AWS Service error publishing message to topic: psr/dev/psr-4g-at/cmd/. Error Code: ForbiddenException
```

## ?? Recursos Adicionales

- [AWS IoT Core Documentation](https://docs.aws.amazon.com/iot/)
- [Configuración de Credenciales AWS](AWS_IoT_Credentials_Setup.md)
- [MQTT Protocol](https://mqtt.org/)
- [AWS IoT Device SDK](https://github.com/aws/aws-iot-device-sdk-dotnet)

## ? Checklist Pre-Uso

- [ ] Credenciales de AWS configuradas
- [ ] Permisos de IoT verificados en IAM
- [ ] Endpoint correcto en appsettings.json
- [ ] Región correcta configurada (us-east-2)
- [ ] JWT token válido para autorización
- [ ] Logs de inicialización verificados
