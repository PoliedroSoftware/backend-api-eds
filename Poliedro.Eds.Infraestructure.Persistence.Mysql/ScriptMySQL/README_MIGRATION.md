# Migración: Campos de Auditoría en hose y hose_history

## Descripción
Esta migración agrega los campos de auditoría (`createdBy`, `createdAt`, `updatedBy`, `updatedAt`) a las tablas `hose` y `hose_history`, y crea el trigger correcto para poblar los campos de auditoría con la fecha y hora actual en zona horaria de Colombia.

## Problema Resuelto
- El campo `createdAt` no registraba la fecha y hora actual en los registros de historial
- Los campos de auditoría no estaban mapeados en la configuración de Entity Framework
- El trigger estaba en la tabla incorrecta (`court_dispensers` en lugar de `hose`)
- La tabla `hose_history` tenía un campo `date` que no debería existir

## Archivos Modificados
1. `DatabaseDump.sql` - Actualizado con la nueva estructura de las tablas y el trigger correcto
2. `HoseHistoryConfiguration.cs` - Agregado el mapeo de los campos de auditoría
3. `migration_add_audit_fields_hose_history.sql` - Script de migración para bases de datos existentes

## Triggers Creados/Modificados
1. `after_hose_update` - **NUEVO** - Crea registros en `hose_history` cuando se actualiza `accumulated_amount` en la tabla `hose`
   - Usa zona horaria de Colombia con `CONVERT_TZ(NOW(), 'UTC', 'America/Bogota')`
   - Registra el usuario de MySQL con `CURRENT_USER()`
   - Solo crea registro si cambió el `accumulated_amount`

## Triggers Eliminados
1. `trg_insert_hose_history` - ELIMINADO (estaba en la tabla incorrecta)
2. `trg_update_hose_history` - ELIMINADO (estaba en la tabla incorrecta)
3. `trg_update_hose_accumulated_on_update` - ELIMINADO (estaba en la tabla incorrecta)

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

2. El trigger se ha creado correctamente:
```sql
SHOW CREATE TRIGGER after_hose_update;
```

3. Los nuevos registros incluyen el campo `createdAt` con la fecha/hora actual:
```sql
SELECT * FROM hose_history ORDER BY id_hose_hose_history DESC LIMIT 5;
```

## Impacto
- **Positivo**: Los registros de auditoría ahora incluirán la fecha y hora de creación real en zona horaria de Colombia
- **Positivo**: Las actualizaciones de la tabla `hose` ahora crearán registros históricos en `hose_history`
- **Compatibilidad**: Los registros existentes tendrán valores NULL en los campos de auditoría (esto es esperado)
- **Sin Breaking Changes**: La aplicación continuará funcionando sin cambios adicionales
- **Comportamiento Nuevo**: Cada actualización del campo `accumulated_amount` en `hose` creará un nuevo registro en `hose_history` para mantener el historial completo de cambios
- **Limpieza**: Se elimina el campo `date` de `hose_history` que no debería existir

## Notas
- Los registros creados antes de esta migración tendrán valores NULL en los campos de auditoría
- Los nuevos registros históricos creados por el trigger `after_hose_update` tendrán:
  - `createdBy = CURRENT_USER()` (usuario de MySQL que ejecuta la operación)
  - `createdAt = CONVERT_TZ(NOW(), 'UTC', 'America/Bogota')` (hora de Colombia UTC-5)
- Los registros en la tabla `hose` creados/actualizados directamente por la aplicación tendrán:
  - `createdBy` / `updatedBy` = nombre del usuario autenticado
  - `createdAt` / `updatedAt` = fecha/hora de Colombia establecida por `AuditableDbContext`
- El trigger solo crea registros históricos cuando cambia el `accumulated_amount`, no en cada actualización
- La zona horaria de Colombia (America/Bogota) es UTC-5
