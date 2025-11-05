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
