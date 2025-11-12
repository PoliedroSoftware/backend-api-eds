# Configuración de Credenciales AWS IoT Core

## ?? Problemas Resueltos

### ? Error 1: "No such host is known"
**Causa**: El servicio intentaba hacer peticiones HTTP sin la firma AWS Signature v4 requerida.  
**Solución**: Ahora usa el SDK oficial `AWSSDK.IotData` que maneja automáticamente la autenticación y firma de peticiones.

### ? Error 2: "Method does not have an implementation"
**Causa**: Incompatibilidad entre versiones 3.x y 4.x del AWS SDK.  
**Solución**: Actualizado a `AWSSDK.IotData` versión 4.0.2.4.

### ? Error 3: "RegionEndpoint property is not applicable"
**Causa**: AWS IoT Data Client no acepta `RegionEndpoint` cuando se especifica `ServiceURL`.  
**Solución**: Configuración corregida para usar solo `ServiceURL` sin `RegionEndpoint`.

---

## ?? Cambios Implementados

### 1. Paquete NuGet agregado
```xml
<PackageReference Include="AWSSDK.IotData" Version="4.0.2.4" />
```

> ?? **Importante**: Asegúrate de usar la versión 4.x para compatibilidad con otros paquetes AWS SDK v4.

### 2. Servicio actualizado
- ? Usa `AmazonIotDataClient` del SDK oficial
- ? Configuración correcta del endpoint (sin RegionEndpoint)
- ? Manejo automático de credenciales AWS
- ? QoS 1 (At least once delivery)
- ? Logging detallado de errores

### 3. Configuración correcta del cliente
```csharp
// ? INCORRECTO - No funciona con IoT Data
var clientConfig = new AmazonIotDataConfig
{
    ServiceURL = $"https://{_endpoint}",
    RegionEndpoint = RegionEndpoint.GetBySystemName(region) // ERROR!
};

// ? CORRECTO - Solo ServiceURL
var clientConfig = new AmazonIotDataConfig
{
    ServiceURL = $"https://{_endpoint}"
};
```

> **Nota**: A diferencia de otros servicios de AWS, `AmazonIotDataClient` deriva la región del endpoint y no requiere (ni acepta) configurar `RegionEndpoint` explícitamente.

---

## ?? Configuración de Credenciales AWS

El servicio usa `FallbackCredentialsFactory` que busca credenciales en el siguiente orden:

### Opción 1: Variables de Ambiente (Recomendado para desarrollo local)

```bash
# Windows (PowerShell)
$env:AWS_ACCESS_KEY_ID="tu_access_key"
$env:AWS_SECRET_ACCESS_KEY="tu_secret_key"
$env:AWS_REGION="us-east-2"

# Windows (CMD)
set AWS_ACCESS_KEY_ID=tu_access_key
set AWS_SECRET_ACCESS_KEY=tu_secret_key
set AWS_REGION=us-east-2

# Linux/Mac
export AWS_ACCESS_KEY_ID="tu_access_key"
export AWS_SECRET_ACCESS_KEY="tu_secret_key"
export AWS_REGION="us-east-2"
```

### Opción 2: Archivo de Credenciales AWS (Recomendado para desarrollo)

**Ubicación del archivo:**
- Windows: `C:\Users\{TuUsuario}\.aws\credentials`
- Linux/Mac: `~/.aws/credentials`

**Contenido del archivo `credentials`:**
```ini
[default]
aws_access_key_id = tu_access_key
aws_secret_access_key = tu_secret_key
```

**Archivo `config` (mismo directorio):**
```ini
[default]
region = us-east-2
output = json
```

### Opción 3: IAM Role (Recomendado para producción en AWS)

Si la aplicación corre en AWS (EC2, ECS, Lambda, etc.), asigna un **IAM Role** con los permisos necesarios.

**Permisos IAM requeridos:**
```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "iot:Publish",
        "iot:Connect"
      ],
      "Resource": [
        "arn:aws:iot:us-east-2:*:topic/psr/*",
        "arn:aws:iot:us-east-2:*:client/*"
      ]
    }
  ]
}
```

---

## ?? Cómo Obtener tus Credenciales AWS

### 1. Desde AWS Console:

1. Ve a [AWS IAM Console](https://console.aws.amazon.com/iam/)
2. Click en **Users** ? Selecciona tu usuario (o crea uno nuevo)
3. Ve a la pestaña **Security credentials**
4. Click en **Create access key**
5. Selecciona **Application running outside AWS**
6. Guarda el **Access Key ID** y **Secret Access Key**

### 2. Verificar acceso a IoT Core:

```bash
# Instalar AWS CLI si no lo tienes
# Windows: https://awscli.amazonaws.com/AWSCLIV2.msi
# Mac: brew install awscli
# Linux: apt-get install awscli

# Configurar AWS CLI
aws configure

# Probar conexión a IoT Core
aws iot describe-endpoint --endpoint-type iot:Data-ATS
```

---

## ? Verificar Configuración

### 1. Verificar que el servicio se inicializa correctamente

Al arrancar la aplicación, deberías ver en los logs:

```
[Information] AWS IoT Data client initialized successfully with endpoint: a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com in region: us-east-2
```

### 2. Si ves errores de credenciales:

```
[Error] Failed to initialize AWS IoT Data client
```

**Solución**: Verifica que las credenciales estén configuradas correctamente.

### 3. Probar publicación:

```bash
POST https://tu-api/api/v1/iot/publish
Authorization: Bearer {token}
Content-Type: application/json

{
  "message": {
    "input1": "test",
    "input2": "connection",
    "output1": "success",
    "output2": "ok"
  },
  "topic": "psr/dev/psr-4g-at/cmd/"
}
```

---

## ?? Logs Detallados

El servicio ahora genera logs más específicos:

### Éxito:
```
[Information] AWS IoT Data client initialized successfully with endpoint: a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com in region: us-east-2
[Information] Attempting to publish message to IoT topic: psr/dev/psr-4g-at/cmd/
[Debug] Message payload: {"input1":"test",...}
[Information] Message published successfully to topic: psr/dev/psr-4g-at/cmd/
```

### Error de credenciales:
```
[Error] AWS Service error publishing message to topic: psr/dev/psr-4g-at/cmd/. Error Code: UnauthorizedException
```

### Error de permisos:
```
[Error] AWS IoT Data error publishing message to topic: psr/dev/psr-4g-at/cmd/. Error Code: ForbiddenException, Status: 403
```

### Error de endpoint:
```
[Error] AWS Service error publishing message to topic: psr/dev/psr-4g-at/cmd/. Error Code: InvalidRequestException
```

---

## ?? Quick Start

### Para desarrollo local:

1. **Crear archivo de credenciales:**
   ```bash
   # Windows
   mkdir %USERPROFILE%\.aws
   notepad %USERPROFILE%\.aws\credentials
   
   # Linux/Mac
   mkdir -p ~/.aws
   nano ~/.aws/credentials
   ```

2. **Agregar credenciales:**
   ```ini
   [default]
   aws_access_key_id = AKIAIOSFODNN7EXAMPLE
   aws_secret_access_key = wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
   ```

3. **Crear archivo de configuración:**
   ```bash
   # Windows
   notepad %USERPROFILE%\.aws\config
   
   # Linux/Mac
   nano ~/.aws/config
   ```

4. **Agregar región:**
   ```ini
   [default]
   region = us-east-2
   ```

5. **Reiniciar la aplicación**

---

## ?? Monitorear en AWS IoT Console

1. Ve a [AWS IoT Console](https://console.aws.amazon.com/iot/)
2. En el menú lateral, selecciona **Test** ? **MQTT test client**
3. En **Subscribe to a topic**, ingresa: `psr/dev/psr-4g-at/cmd/#`
4. Click **Subscribe**
5. Envía un mensaje desde tu API
6. Deberías ver el mensaje aparecer en tiempo real

---

## ??? Troubleshooting

### Error: "RegionEndpoint property is not applicable for AmazonIotDataClient" ? RESUELTO
**Causa**: Intentar configurar `RegionEndpoint` junto con `ServiceURL`.  
**Solución**: Solo usar `ServiceURL` en la configuración del cliente.

### Error: "Method 'DetermineServiceOperationEndpoint' does not have an implementation" ? RESUELTO
**Causa**: Incompatibilidad entre versiones 3.x y 4.x del AWS SDK.  
**Solución**: Actualizado a `AWSSDK.IotData` versión 4.0.2.4.

### Error: "Unable to get IAM security credentials"
- Verifica que las credenciales estén en `~/.aws/credentials`
- O que las variables de ambiente estén configuradas
- O que el IAM Role esté asignado (si estás en AWS)

### Error: "UnauthorizedException"
- Las credenciales son incorrectas o están vencidas
- Genera nuevas credenciales en AWS IAM

### Error: "ForbiddenException"
- El usuario/role no tiene permisos de `iot:Publish`
- Agrega la policy de IoT al usuario/role

### Error: "InvalidRequestException"
- Verifica que el endpoint sea correcto
- Verifica que el topic sea válido (sin espacios ni caracteres especiales)

### Mensaje no aparece en AWS IoT Console
- Verifica que estés suscrito al topic correcto
- Usa wildcard: `psr/dev/psr-4g-at/cmd/#`
- Verifica los logs de la aplicación para confirmar que se envió

---

## ?? Información de tu Configuración

**Endpoint**: `a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com`  
**Region**: `us-east-2`  
**Default Topic**: `psr/dev/psr-4g-at/cmd/`  
**Client ID** (para referencia): `iotconsole-a4fcac7a-5de8-4e37-8b0d-0d0565e18d6d`

---

## ? Checklist de Configuración

- [ ] Instalar AWS CLI (opcional pero recomendado)
- [ ] Crear/obtener credenciales de AWS IAM
- [ ] Configurar credenciales (archivo o variables de ambiente)
- [ ] Verificar permisos de IoT en IAM
- [ ] Limpiar y reconstruir el proyecto (`dotnet clean` y `dotnet build`)
- [ ] Reiniciar la aplicación
- [ ] Verificar logs de inicialización
- [ ] Probar endpoint de publicación
- [ ] Monitorear en AWS IoT Console

---

## ?? Enlaces Útiles

- [AWS IoT Core Documentation](https://docs.aws.amazon.com/iot/)
- [AWS CLI Installation](https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html)
- [AWS Credentials Configuration](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html)
- [AWS IoT Policies](https://docs.aws.amazon.com/iot/latest/developerguide/iot-policies.html)
- [AWS SDK for .NET v4](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/welcome.html)
- [AWS IoT Data Plane API](https://docs.aws.amazon.com/iot/latest/apireference/API_Operations_AWS_IoT_Data_Plane.html)
