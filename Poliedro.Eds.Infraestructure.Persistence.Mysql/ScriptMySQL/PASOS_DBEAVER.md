# Pasos para Ejecutar la Migración en DBeaver

## Contexto
Este script actualiza los triggers de las tablas `hose`, `product` y `tank` para que registren correctamente el usuario de la aplicación y la hora de Colombia en las tablas de historial.

## Pasos en DBeaver

### 1. Conectarse a la Base de Datos
1. Abrir DBeaver
2. Conectarse a la base de datos `eds`

### 2. Abrir el Script SQL
1. En DBeaver, hacer clic derecho en la conexión de `eds`
2. Seleccionar **SQL Editor** → **Open SQL Script**
3. Navegar hasta el archivo: `fix_history_triggers_v2.sql`
4. O copiar el contenido del script directamente

### 3. Ejecutar el Script Completo
1. Con el script abierto en DBeaver
2. Hacer clic en el botón **Execute Script** (icono de "play" con documento) o presionar `Ctrl+Alt+X`
3. **IMPORTANTE**: NO usar "Execute Statement" (Ctrl+Enter), usar "Execute Script" completo

### 4. Verificar que se Ejecutó Correctamente
Ejecutar esta consulta para verificar que los triggers fueron actualizados:

```sql
SELECT 
    TRIGGER_NAME, 
    EVENT_OBJECT_TABLE,
    ACTION_STATEMENT
FROM information_schema.TRIGGERS
WHERE TRIGGER_SCHEMA = 'eds'
  AND EVENT_OBJECT_TABLE IN ('hose', 'product', 'tank')
ORDER BY EVENT_OBJECT_TABLE;
```

Deberías ver en el `ACTION_STATEMENT`:
- `@app_user` (para capturar el usuario de la aplicación)
- `CONVERT_TZ` (para convertir a hora de Colombia)

### 5. Reiniciar la Aplicación
Después de ejecutar el script en la base de datos, reiniciar el servicio de la aplicación para que tome los cambios.

## Contenido del Script a Ejecutar

El script completo está en: `fix_history_triggers_v2.sql`

O puedes copiar este contenido directamente en DBeaver:

```sql
-- Migration: Fix history triggers to use @app_user and Colombia timezone
-- Date: 2025-11-05
-- Purpose: Update existing triggers to capture application user and correct timezone

-- Drop existing triggers
DROP TRIGGER IF EXISTS `after_hose_update`;
DROP TRIGGER IF EXISTS `after_product_update`;
DROP TRIGGER IF EXISTS `after_tank_update`;

DELIMITER $$

-- Trigger for hose table updates
CREATE TRIGGER `after_hose_update` 
AFTER UPDATE ON `hose` 
FOR EACH ROW 
BEGIN
    DECLARE v_current_user VARCHAR(255);
    DECLARE v_current_datetime DATETIME;
    
    -- Solo guardar en el historial si cambió el accumulated_amount
    IF (OLD.accumulated_amount <> NEW.accumulated_amount) THEN
        -- Get the application user from session variable
        -- If not set, fall back to CURRENT_USER()
        SET v_current_user = IFNULL(@app_user, CURRENT_USER());
        
        -- Get current timestamp in Colombia timezone (UTC-5)
        SET v_current_datetime = CONVERT_TZ(NOW(), '+00:00', '-05:00');
        
        INSERT INTO hose_history (
            id_hose, id_dispensers, accumulated_amount, accumulated_gallons,
            createdBy, createdAt, updatedBy, updatedAt
        )
        VALUES (
            OLD.id_hose,
            OLD.id_dispensers,
            NEW.accumulated_amount,
            NEW.accumulated_gallons,
            v_current_user,   -- Application user
            v_current_datetime,    -- Colombia time
            NULL,     -- updated_by no aplica en histórico
            NULL      -- updated_at no aplica en histórico
        );
    END IF;
END$$

-- Trigger for product table updates
CREATE TRIGGER `after_product_update` 
AFTER UPDATE ON `product` 
FOR EACH ROW 
BEGIN
    DECLARE v_current_user VARCHAR(255);
    DECLARE v_current_datetime DATETIME;
    
    -- Solo guardar en el historial si cambia stock, sell_price o purchase_price
    IF OLD.stock <> NEW.stock
       OR OLD.sell_price <> NEW.sell_price
       OR OLD.purchase_price <> NEW.purchase_price THEN
       
        -- Get the application user from session variable
        SET v_current_user = IFNULL(@app_user, CURRENT_USER());
        
        -- Get current timestamp in Colombia timezone (UTC-5)
        SET v_current_datetime = CONVERT_TZ(NOW(), '+00:00', '-05:00');
        
        INSERT INTO product_history (
            id_product,
            old_sell_price, new_sell_price,
            old_stock, new_stock,
            old_purchase_price, new_purchase_price,
            date,
            createdBy, createdAt, updatedBy, updatedAt
        )
        VALUES (
            OLD.id_product,
            OLD.sell_price, NEW.sell_price,
            OLD.stock, NEW.stock,
            OLD.purchase_price, NEW.purchase_price,
            v_current_datetime,  -- Use Colombia time for date field
            v_current_user,   -- Application user
            v_current_datetime,    -- Colombia time
            NULL,     -- updated_by no aplica en histórico
            NULL      -- updated_at no aplica en histórico
        );
    END IF;
END$$

-- Trigger for tank table updates
CREATE TRIGGER `after_tank_update` 
AFTER UPDATE ON `tank` 
FOR EACH ROW 
BEGIN
    DECLARE v_current_user VARCHAR(255);
    DECLARE v_current_datetime DATETIME;
    
    -- Solo guardar en el historial si cambió el ability
    IF OLD.ability <> NEW.ability THEN
        -- Get the application user from session variable
        SET v_current_user = IFNULL(@app_user, CURRENT_USER());
        
        -- Get current timestamp in Colombia timezone (UTC-5)
        SET v_current_datetime = CONVERT_TZ(NOW(), '+00:00', '-05:00');
        
        INSERT INTO tank_history (
            id_tank, old_ability, new_ability,
            createdBy, createdAt, updatedBy, updatedAt
        )
        VALUES (
            OLD.id_tank,
            OLD.ability,
            NEW.ability,
            v_current_user,   -- Application user
            v_current_datetime,    -- Colombia time
            NULL,     -- updated_by no aplica en histórico
            NULL      -- updated_at no aplica en histórico
        );
    END IF;
END$$

DELIMITER ;
```

## Prueba Después de la Migración

### 1. Probar actualización de hose
1. Actualizar un registro en la tabla `hose` desde la aplicación
2. Verificar en `hose_history`:
```sql
SELECT id_hose, createdBy, createdAt
FROM hose_history
ORDER BY id_hose_hose_history DESC
LIMIT 5;
```

**Resultado esperado:**
- `createdBy` debe mostrar "admin" (o el usuario que hizo la actualización)
- `createdAt` debe mostrar hora de Colombia (NO +5 horas)

### 2. Probar actualización de product
1. Actualizar un registro en la tabla `product` desde la aplicación
2. Verificar en `product_history`:
```sql
SELECT id_product, createdBy, createdAt
FROM product_history
ORDER BY id_product_history DESC
LIMIT 5;
```

**Resultado esperado:**
- `createdBy` debe mostrar "admin" (NO "root@IP")
- `createdAt` debe mostrar hora de Colombia

### 3. Probar actualización de tank
1. Actualizar un registro en la tabla `tank` desde la aplicación
2. Verificar que NO hay error 500
3. Verificar en `tank_history`:
```sql
SELECT id_tank, createdBy, createdAt
FROM tank_history
ORDER BY id_tank_history DESC
LIMIT 5;
```

## Notas Importantes

1. **El script debe ejecutarse COMPLETO** - No ejecutar línea por línea
2. **Usar "Execute Script"** en DBeaver, no "Execute Statement"
3. **Reiniciar la aplicación** después de ejecutar el script
4. **El fix del tank** también requiere desplegar el código actualizado de la aplicación

## ¿Qué hace el script?

1. **Elimina los triggers antiguos** que usaban `CURRENT_USER()` y `NOW()`
2. **Crea triggers nuevos** que:
   - Leen el usuario desde `@app_user` (variable de sesión que la aplicación establece)
   - Si `@app_user` no está establecida, usan `CURRENT_USER()` como respaldo
   - Convierten la hora de UTC a hora de Colombia (UTC-5)
3. **Insertan los valores correctos** en las tablas de historial

## Solución de Problemas

### Error: "DELIMITER command not supported"
- Copiar el script sin las líneas `DELIMITER`
- Ejecutar los CREATE TRIGGER directamente

### Los triggers no se actualizaron
- Verificar con la consulta del paso 4
- Si aún ves `CURRENT_USER()` en ACTION_STATEMENT, el script no se ejecutó correctamente

### Sigue mostrando "root@IP" después de ejecutar
- Verificar que la aplicación fue reiniciada
- Verificar que los triggers fueron actualizados (consulta del paso 4)
