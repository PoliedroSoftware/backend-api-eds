-- Verification and Deployment Script for History Triggers
-- Date: 2025-11-10
-- Purpose: Verify current state and deploy updated triggers with proper user capture and timezone

-- Step 1: Check if triggers exist
SELECT 
    TRIGGER_NAME, 
    EVENT_MANIPULATION, 
    EVENT_OBJECT_TABLE,
    ACTION_TIMING,
    DEFINER
FROM information_schema.TRIGGERS
WHERE TRIGGER_SCHEMA = DATABASE()
  AND EVENT_OBJECT_TABLE IN ('hose', 'product', 'tank')
ORDER BY EVENT_OBJECT_TABLE, TRIGGER_NAME;

-- Step 2: Show current trigger definitions (for backup)
SHOW CREATE TRIGGER after_hose_update;
SHOW CREATE TRIGGER after_product_update;
SHOW CREATE TRIGGER after_tank_update;

-- Step 3: Check @app_user session variable (should be NULL if not set by application)
SELECT @app_user AS current_app_user;

-- Step 4: Test timezone conversion
SELECT 
    NOW() AS utc_time,
    CONVERT_TZ(NOW(), '+00:00', '-05:00') AS colombia_time,
    TIMESTAMPDIFF(HOUR, CONVERT_TZ(NOW(), '+00:00', '-05:00'), NOW()) AS hour_difference;

-- Step 5: Check history table structures
DESCRIBE hose_history;
DESCRIBE product_history;
DESCRIBE tank_history;

-- Step 6: Check recent history entries to see current user format
SELECT 'hose_history recent entries' AS table_name;
SELECT id_hose_hose_history, id_hose, createdBy, createdAt, updatedBy, updatedAt
FROM hose_history
ORDER BY id_hose_hose_history DESC
LIMIT 5;

SELECT 'product_history recent entries' AS table_name;
SELECT id_product_history, id_product, createdBy, createdAt, date
FROM product_history
ORDER BY id_product_history DESC
LIMIT 5;

SELECT 'tank_history recent entries' AS table_name;
SELECT id_tank_history, id_tank, createdBy, createdAt
FROM tank_history
ORDER BY id_tank_history DESC
LIMIT 5;

-- =============================================================================
-- DEPLOYMENT SECTION - Drop and recreate triggers
-- =============================================================================

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

-- =============================================================================
-- VERIFICATION SECTION - Confirm triggers were created
-- =============================================================================

SELECT 'Triggers after deployment:' AS status;

SELECT 
    TRIGGER_NAME, 
    EVENT_MANIPULATION, 
    EVENT_OBJECT_TABLE,
    ACTION_TIMING,
    DEFINER
FROM information_schema.TRIGGERS
WHERE TRIGGER_SCHEMA = DATABASE()
  AND EVENT_OBJECT_TABLE IN ('hose', 'product', 'tank')
ORDER BY EVENT_OBJECT_TABLE, TRIGGER_NAME;

-- Show the new trigger definitions
SHOW CREATE TRIGGER after_hose_update\G
SHOW CREATE TRIGGER after_product_update\G
SHOW CREATE TRIGGER after_tank_update\G

SELECT 'Deployment completed successfully!' AS message;
