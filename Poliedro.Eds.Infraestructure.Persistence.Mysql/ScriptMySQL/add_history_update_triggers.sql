-- Migration: Add history tables and UPDATE triggers for audit trail
-- Date: 2025-11-01
-- Purpose: Create product_history, tank_history tables and UPDATE triggers for hose, product, tank
-- to properly capture application user and use correct timezone

-- Step 1: Create product_history table
CREATE TABLE IF NOT EXISTS `product_history` (
  `id_product_history` INT NOT NULL AUTO_INCREMENT,
  `id_product` INT NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `id_product_type` INT NOT NULL,
  `purchase_price` DOUBLE NULL,
  `sell_price` DOUBLE NULL,
  `stock` DOUBLE NULL,
  `date` DATE NOT NULL,
  `createdBy` VARCHAR(255) NULL,
  `createdAt` DATETIME NULL,
  `updatedBy` VARCHAR(255) NULL,
  `updatedAt` DATETIME NULL,
  PRIMARY KEY (`id_product_history`),
  INDEX `idx_product_history_id_product` (`id_product` ASC),
  INDEX `idx_product_history_date` (`date` ASC)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- Step 2: Create tank_history table
CREATE TABLE IF NOT EXISTS `tank_history` (
  `id_tank_history` INT NOT NULL AUTO_INCREMENT,
  `id_tank` INT NOT NULL,
  `number` VARCHAR(45) NOT NULL,
  `compartment` INT NOT NULL,
  `ability` DOUBLE NOT NULL,
  `stock` DOUBLE NULL,
  `date` DATE NOT NULL,
  `createdBy` VARCHAR(255) NULL,
  `createdAt` DATETIME NULL,
  `updatedBy` VARCHAR(255) NULL,
  `updatedAt` DATETIME NULL,
  PRIMARY KEY (`id_tank_history`),
  INDEX `idx_tank_history_id_tank` (`id_tank` ASC),
  INDEX `idx_tank_history_date` (`date` ASC)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- Step 3: Add hose_history table if it doesn't exist (for new installations)
CREATE TABLE IF NOT EXISTS `hose_history` (
  `id_hose_hose_history` INT NOT NULL AUTO_INCREMENT,
  `id_hose` INT NOT NULL,
  `id_dispensers` INT NOT NULL,
  `accumulated_amount` DOUBLE NOT NULL,
  `accumulated_gallons` DOUBLE NOT NULL,
  `number` VARCHAR(45) NULL,
  `date` DATE NOT NULL,
  `createdBy` VARCHAR(255) NULL,
  `createdAt` DATETIME NULL,
  `updatedBy` VARCHAR(255) NULL,
  `updatedAt` DATETIME NULL,
  PRIMARY KEY (`id_hose_hose_history`),
  INDEX `idx_hose_history_id_hose` (`id_hose` ASC),
  INDEX `idx_hose_history_date` (`date` ASC)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- Step 4: Drop existing UPDATE triggers if they exist
DROP TRIGGER IF EXISTS `trg_update_hose_history`;
DROP TRIGGER IF EXISTS `trg_update_product_history`;
DROP TRIGGER IF EXISTS `trg_update_tank_history`;

-- Step 5: Create UPDATE trigger for hose table
DELIMITER $$

CREATE TRIGGER `trg_update_hose_history`
AFTER UPDATE ON `hose`
FOR EACH ROW
BEGIN
    DECLARE v_current_user VARCHAR(255);
    DECLARE v_current_datetime DATETIME;
    DECLARE v_id_dispensers INT;

    -- Get the application user from session variable (set by application)
    -- If not set, fall back to CURRENT_USER()
    SET v_current_user = IFNULL(@app_user, CURRENT_USER());
    
    -- Get current timestamp in Colombia timezone (UTC-5)
    -- Using CONVERT_TZ to convert from UTC to America/Bogota
    SET v_current_datetime = CONVERT_TZ(NOW(), '+00:00', '-05:00');
    
    -- Get dispenser ID
    SELECT id_dispensers INTO v_id_dispensers
    FROM hose
    WHERE id_hose = NEW.id_hose
    LIMIT 1;

    -- Insert history record
    INSERT INTO hose_history (
        id_hose,
        id_dispensers,
        accumulated_amount,
        accumulated_gallons,
        number,
        date,
        updatedBy,
        updatedAt
    ) VALUES (
        NEW.id_hose,
        IFNULL(v_id_dispensers, NEW.id_dispensers),
        NEW.accumulated_amount,
        NEW.accumulated_gallons,
        NEW.number,
        CURDATE(),
        v_current_user,
        v_current_datetime
    );
END$$

-- Step 6: Create UPDATE trigger for product table
CREATE TRIGGER `trg_update_product_history`
AFTER UPDATE ON `product`
FOR EACH ROW
BEGIN
    DECLARE v_current_user VARCHAR(255);
    DECLARE v_current_datetime DATETIME;

    -- Get the application user from session variable
    SET v_current_user = IFNULL(@app_user, CURRENT_USER());
    
    -- Get current timestamp in Colombia timezone (UTC-5)
    SET v_current_datetime = CONVERT_TZ(NOW(), '+00:00', '-05:00');

    -- Insert history record
    INSERT INTO product_history (
        id_product,
        name,
        id_product_type,
        purchase_price,
        sell_price,
        stock,
        date,
        updatedBy,
        updatedAt
    ) VALUES (
        NEW.id_product,
        NEW.name,
        NEW.id_product_type,
        NEW.purchase_price,
        NEW.sell_price,
        NEW.stock,
        CURDATE(),
        v_current_user,
        v_current_datetime
    );
END$$

-- Step 7: Create UPDATE trigger for tank table
CREATE TRIGGER `trg_update_tank_history`
AFTER UPDATE ON `tank`
FOR EACH ROW
BEGIN
    DECLARE v_current_user VARCHAR(255);
    DECLARE v_current_datetime DATETIME;

    -- Get the application user from session variable
    SET v_current_user = IFNULL(@app_user, CURRENT_USER());
    
    -- Get current timestamp in Colombia timezone (UTC-5)
    SET v_current_datetime = CONVERT_TZ(NOW(), '+00:00', '-05:00');

    -- Insert history record
    INSERT INTO tank_history (
        id_tank,
        number,
        compartment,
        ability,
        stock,
        date,
        updatedBy,
        updatedAt
    ) VALUES (
        NEW.id_tank,
        NEW.number,
        NEW.compartment,
        NEW.ability,
        NEW.stock,
        CURDATE(),
        v_current_user,
        v_current_datetime
    );
END$$

DELIMITER ;

-- Step 8: Update existing hose_history trigger to use Colombia timezone
DROP TRIGGER IF EXISTS `trg_insert_hose_history`;

DELIMITER $$

CREATE TRIGGER `trg_insert_hose_history`
AFTER INSERT ON `court_dispensers`
FOR EACH ROW
BEGIN
    DECLARE court_date DATE;
    DECLARE v_id_dispensers INT;
    DECLARE v_current_user VARCHAR(255);
    DECLARE v_current_datetime DATETIME;

    SELECT date_starttime INTO court_date
    FROM court
    WHERE id_court = NEW.id_court;

    SELECT id_dispensers INTO v_id_dispensers
    FROM hose
    WHERE id_hose = NEW.id_hose;

    -- Get the application user from session variable (set by application)
    -- If not set, fall back to CURRENT_USER()
    SET v_current_user = IFNULL(@app_user, CURRENT_USER());
    
    -- Get current timestamp in Colombia timezone (UTC-5)
    SET v_current_datetime = CONVERT_TZ(NOW(), '+00:00', '-05:00');

    INSERT INTO hose_history (
        id_hose,
        id_dispensers,
        accumulated_amount,
        accumulated_gallons,
        date,
        createdBy,
        createdAt
    ) VALUES (
        NEW.id_hose,
        v_id_dispensers,
        NEW.accumulated_amount,
        NEW.accumulated_gallons,
        court_date,
        v_current_user,
        v_current_datetime
    );
END$$

DELIMITER ;
