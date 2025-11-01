# Migración: Campos de Auditoría en hose_history

## Descripción
Esta migración agrega los campos de auditoría (`createdBy`, `createdAt`, `updatedBy`, `updatedAt`) a la tabla `hose_history` y actualiza el trigger `trg_insert_hose_history` para poblar correctamente el campo `createdAt` con la fecha y hora actual.

## Problema Resuelto
- El campo `createdAt` no registraba la fecha y hora actual en los registros creados por el trigger
- Los campos de auditoría no estaban mapeados en la configuración de Entity Framework

## Archivos Modificados
1. `DatabaseDump.sql` - Actualizado con la nueva estructura de la tabla y el trigger modificado
2. `HoseHistoryConfiguration.cs` - Agregado el mapeo de los campos de auditoría
3. `migration_add_audit_fields_hose_history.sql` - Script de migración para bases de datos existentes

## Instrucciones para Aplicar la Migración

### Base de Datos Principal y QA
Ejecutar el siguiente script en ambas bases de datos:

```bash
mysql -u [usuario] -p [nombre_base_datos] < migration_add_audit_fields_hose_history.sql
```

O desde el cliente MySQL:
```sql
SOURCE /ruta/a/migration_add_audit_fields_hose_history.sql;
```

### Verificación
Después de aplicar la migración, verificar que:

1. Los campos de auditoría existen en la tabla:
```sql
DESCRIBE hose_history;
```

2. El trigger se ha actualizado correctamente:
```sql
SHOW CREATE TRIGGER trg_insert_hose_history;
```

3. Los nuevos registros incluyen el campo `createdAt` con la fecha/hora actual:
```sql
SELECT * FROM hose_history ORDER BY id_hose_hose_history DESC LIMIT 5;
```

## Impacto
- **Positivo**: Los registros de auditoría ahora incluirán la fecha y hora de creación real
- **Compatibilidad**: Los registros existentes tendrán valores NULL en los campos de auditoría (esto es esperado)
- **Sin Breaking Changes**: La aplicación continuará funcionando sin cambios adicionales

## Notas
- Los registros creados antes de esta migración tendrán valores NULL en los campos de auditoría
- Los nuevos registros creados por el trigger tendrán:
  - `createdBy = 'trigger'` (los triggers de base de datos no tienen acceso al contexto de autenticación de la aplicación)
  - `createdAt = CONVERT_TZ(NOW(), 'UTC', 'America/Bogota')` (hora de Colombia, coherente con la aplicación)
- Los registros creados directamente por la aplicación a través de la API tendrán:
  - `createdBy` = nombre del usuario autenticado
  - `createdAt` = fecha/hora de Colombia establecida por `AuditableDbContext`
- La zona horaria de Colombia (America/Bogota) es UTC-5
