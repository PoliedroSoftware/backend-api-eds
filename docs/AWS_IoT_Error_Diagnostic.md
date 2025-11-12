# ?? Diagnóstico de Error: Failed to Publish IoT Message

## ? Error Recibido
```json
{
    "statusCode": 500,
    "success": false,
    "message": null,
    "data": "Failed to publish message to IoT Core"
}
```

## ?? Análisis del Error

Basado en los logs de Debug, se lanzó una excepción `HttpRequestException`. Esto indica uno de los siguientes problemas:

### 1. **Credenciales de AWS NO Configuradas** (Más Probable)

#### Síntoma en logs:
```
[Error] Failed to load AWS credentials. Make sure credentials are configured.
[Error] Please configure AWS credentials using one of these methods:
```

#### ? Solución Inmediata:

**Opción A: Variables de Ambiente (Rápido para testing)**
```powershell
# PowerShell
$env:AWS_ACCESS_KEY_ID="AKIAIOSFODNN7EXAMPLE"
$env:AWS_SECRET_ACCESS_KEY="wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY"
$env:AWS_REGION="us-east-2"
```

**Opción B: Archivo de Credenciales (Recomendado)**
```powershell
# Crear directorio
mkdir $env:USERPROFILE\.aws

# Crear archivo credentials
@"
[default]
aws_access_key_id = AKIAIOSFODNN7EXAMPLE
aws_secret_access_key = wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
"@ | Out-File -FilePath "$env:USERPROFILE\.aws\credentials" -Encoding ASCII

# Crear archivo config
@"
[default]
region = us-east-2
output = json
"@ | Out-File -FilePath "$env:USERPROFILE\.aws\config" -Encoding ASCII
```

---

### 2. **Endpoint Incorrecto o Inalcanzable**

#### Síntoma:
```
System.Net.Http.HttpRequestException
```

#### Logs esperados:
```
[Error] HTTP request error publishing message
[Error] Check:
[Error] - Internet connection
[Error] - Endpoint is correct: a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com
```

#### ? Solución:
1. Verificar endpoint en `appsettings.json`:
```json
"AWS": {
  "IoT": {
    "Endpoint": "a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com"
  }
}
```

2. Probar conectividad:
```powershell
# PowerShell
Test-NetConnection -ComputerName a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com -Port 443
```

---

### 3. **Permisos de IAM Insuficientes**

#### Síntoma en logs:
```
[Error] AWS IoT Data error: UnauthorizedException or ForbiddenException
```

#### ? Solución:
Verificar que el usuario de AWS IAM tenga esta policy:

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
        "arn:aws:iot:us-east-2:*:topic/psr/*"
      ]
    }
  ]
}
```

---

## ?? Pasos de Diagnóstico Rápido

### Paso 1: Verificar Logs de Inicialización

Reinicia la aplicación y busca en los logs:

? **Exitoso:**
```
[Information] Initializing AWS IoT Data client...
[Information] AWS credentials loaded successfully
[Information] AWS IoT Data client initialized successfully
```

? **Con Error:**
```
[Error] Failed to load AWS credentials
[Error] Failed to initialize AWS IoT Data client
```

### Paso 2: Si falló la inicialización

```powershell
# 1. Verificar credenciales de AWS
aws configure list

# 2. Si AWS CLI no está configurado, configurarlo
aws configure

# 3. Probar conexión a IoT
aws iot describe-endpoint --endpoint-type iot:Data-ATS --region us-east-2
```

**Respuesta esperada:**
```json
{
    "endpointAddress": "a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com"
}
```

### Paso 3: Configurar Credenciales Correctas

Si el comando anterior falla con "Unable to locate credentials":

1. **Obtener credenciales desde AWS Console:**
   - Ir a AWS IAM ? Users ? Tu usuario ? Security credentials
   - Click "Create access key"
   - Guardar Access Key ID y Secret Access Key

2. **Configurar credenciales:**
```powershell
aws configure
# AWS Access Key ID: [Pegar tu Access Key]
# AWS Secret Access Key: [Pegar tu Secret Key]
# Default region name: us-east-2
# Default output format: json
```

### Paso 4: Reiniciar la Aplicación

Después de configurar credenciales, reinicia la aplicación y revisa los logs.

### Paso 5: Probar el Endpoint

```bash
POST /api/v1/iot/publish
Authorization: Bearer {tu_token_jwt}
Content-Type: application/json

{
  "message": {
    "input1": "test_connection",
    "input2": "diagnostic",
    "output1": "success",
    "output2": "ok"
  },
  "topic": "psr/dev/psr-4g-at/cmd/"
}
```

---

## ?? Logs Detallados Ahora Disponibles

Con las mejoras implementadas, ahora verás logs más específicos:

### Durante Inicialización:
```
[Information] Initializing AWS IoT Data client...
[Information] Endpoint: a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com
[Information] Region: us-east-2
[Information] AWS credentials loaded successfully
[Debug] Access Key ID: AKIA...
[Information] AWS IoT Data client initialized successfully
```

### Durante Publicación:
```
[Information] Attempting to publish message to IoT topic: psr/dev/psr-4g-at/cmd/
[Debug] Endpoint: a2fzkzjqfy7bda-ats.iot.us-east-2.amazonaws.com
[Debug] Message payload: {"input1":"test",...}
[Debug] Payload size: 123 bytes
[Debug] Sending publish request to AWS IoT Core...
[Debug] Response received. Status: 200
[Information] Message published successfully to topic: psr/dev/psr-4g-at/cmd/
```

### Errores Específicos:
```
[Error] AWS IoT Data error: UnauthorizedException
[Error] Authentication/Authorization Error:
[Error] - Check that AWS credentials are correctly configured
[Error] - Verify credentials have not expired
[Error] - Ensure IAM user/role has iot:Publish permission
```

---

## ? Checklist de Solución

- [ ] Verificar que AWS CLI esté instalado
- [ ] Ejecutar `aws configure` y configurar credenciales
- [ ] Verificar que las credenciales sean válidas con `aws sts get-caller-identity`
- [ ] Verificar permisos IAM (iot:Publish)
- [ ] Verificar endpoint en appsettings.json
- [ ] Reiniciar la aplicación
- [ ] Verificar logs de inicialización
- [ ] Probar el endpoint de publicación
- [ ] Monitorear en AWS IoT Console (MQTT test client)

---

## ?? Enlaces Útiles

- [Instalar AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html)
- [Configurar AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-quickstart.html)
- [AWS IoT Policies](https://docs.aws.amazon.com/iot/latest/developerguide/iot-policies.html)
- [Troubleshooting AWS Credentials](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-troubleshooting.html)

---

## ?? Solución Más Común

**El 90% de los casos de este error se debe a credenciales no configuradas.**

**Solución rápida (5 minutos):**

1. Instalar AWS CLI: https://awscli.amazonaws.com/AWSCLIV2.msi
2. Abrir PowerShell y ejecutar: `aws configure`
3. Ingresar tus credenciales cuando se solicite
4. Reiniciar la aplicación
5. ? Debería funcionar

**Si no tienes credenciales:**
- Ve a AWS Console ? IAM ? Users ? Create access key
- Usa esas credenciales en `aws configure`
