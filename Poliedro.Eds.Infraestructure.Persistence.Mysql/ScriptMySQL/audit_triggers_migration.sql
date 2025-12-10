-- Migration script to add audit fields and update triggers for history tables
-- This script adds CreatedBy and CreatedAt fields to history tables and creates/updates triggers
-- to use session variables (@app_user, @app_timestamp) for proper audit trail

USE eds_new;

-- =========================================
-- 1. Add audit fields to hose_history table
-- =========================================

-- Add CreatedBy column if not exists
SET @column_check = (
    SELECT COUNT(*)
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
    AND TABLE_NAME = 'hose_history'
    AND COLUMN_NAME = 'createdBy'
);

SET @sql = IF(@column_check = 0,
    'ALTER TABLE hose_history ADD COLUMN createdBy VARCHAR(255) NULL AFTER date',
    'SELECT "Column createdBy already exists in hose_history" AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Add CreatedAt column if not exists
SET @column_check = (
    SELECT COUNT(*)
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
    AND TABLE_NAME = 'hose_history'
    AND COLUMN_NAME = 'createdAt'
);

SET @sql = IF(@column_check = 0,
    'ALTER TABLE hose_history ADD COLUMN createdAt DATETIME NULL AFTER createdBy',
    'SELECT "Column createdAt already exists in hose_history" AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Add UpdatedBy column if not exists
SET @column_check = (
    SELECT COUNT(*)
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
    AND TABLE_NAME = 'hose_history'
    AND COLUMN_NAME = 'updatedBy'
);

SET @sql = IF(@column_check = 0,
    'ALTER TABLE hose_history ADD COLUMN updatedBy VARCHAR(255) NULL AFTER createdAt',
    'SELECT "Column updatedBy already exists in hose_history" AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Add UpdatedAt column if not exists
SET @column_check = (
    SELECT COUNT(*)
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
    AND TABLE_NAME = 'hose_history'
    AND COLUMN_NAME = 'updatedAt'
);

SET @sql = IF(@column_check = 0,
    'ALTER TABLE hose_history ADD COLUMN updatedAt DATETIME NULL AFTER updatedBy',
    'SELECT "Column updatedAt already exists in hose_history" AS message'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- =========================================
-- 2. Create product_history table
-- =========================================

CREATE TABLE IF NOT EXISTS product_history (
    id_product_history INT NOT NULL AUTO_INCREMENT,
    id_product INT NOT NULL,
    name VARCHAR(45) NOT NULL,
    id_product_type INT NOT NULL,
    stock DOUBLE NULL,
    sell_price DOUBLE NULL,
    purchase_price DOUBLE NULL,
    date DATETIME NOT NULL,
    createdBy VARCHAR(255) NULL,
    createdAt DATETIME NULL,
    updatedBy VARCHAR(255) NULL,
    updatedAt DATETIME NULL,
    PRIMARY KEY (id_product_history),
    KEY fk_product_history_product_idx (id_product),
    CONSTRAINT fk_product_history_product FOREIGN KEY (id_product) REFERENCES product (id_product)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- =========================================
-- 3. Create tank_history table
-- =========================================

CREATE TABLE IF NOT EXISTS tank_history (
    id_tank_history INT NOT NULL AUTO_INCREMENT,
    id_tank INT NOT NULL,
    number VARCHAR(45) NOT NULL,
    ability DOUBLE NOT NULL,
    compartment INT NOT NULL,
    stock DOUBLE NOT NULL,
    date DATETIME NOT NULL,
    createdBy VARCHAR(255) NULL,
    createdAt DATETIME NULL,
    updatedBy VARCHAR(255) NULL,
    updatedAt DATETIME NULL,
    PRIMARY KEY (id_tank_history),
    KEY fk_tank_history_tank_idx (id_tank),
    CONSTRAINT fk_tank_history_tank FOREIGN KEY (id_tank) REFERENCES tank (id_tank)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- =========================================
-- 4. Update existing trg_insert_hose_history trigger
-- =========================================

DROP TRIGGER IF EXISTS trg_insert_hose_history;

DELIMITER $$

CREATE TRIGGER trg_insert_hose_history
AFTER INSERT ON court_dispensers
FOR EACH ROW
BEGIN
    DECLARE court_date DATE;
    DECLARE v_id_dispensers INT;

    SELECT date_starttime INTO court_date
    FROM court
    WHERE id_court = NEW.id_court;

    SELECT id_dispensers INTO v_id_dispensers
    FROM hose
    WHERE id_hose = NEW.id_hose;

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
        COALESCE(@app_user, CURRENT_USER()),
        COALESCE(@app_timestamp, NOW())
    );
END$$

DELIMITER ;

-- =========================================
-- 5. Create afterhoseupdate trigger
-- =========================================

DROP TRIGGER IF EXISTS afterhoseupdate;

DELIMITER $$

CREATE TRIGGER afterhoseupdate
AFTER UPDATE ON hose
FOR EACH ROW
BEGIN
    -- Only insert into history if accumulated values changed
    IF OLD.accumulated_amount <> NEW.accumulated_amount 
       OR OLD.accumulated_gallons <> NEW.accumulated_gallons THEN
        
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
            NEW.id_dispensers,
            NEW.accumulated_amount,
            NEW.accumulated_gallons,
            NOW(),
            COALESCE(@app_user, CURRENT_USER()),
            COALESCE(@app_timestamp, NOW())
        );
    END IF;
END$$

DELIMITER ;

-- =========================================
-- 6. Create afterproductupdate trigger
-- =========================================

DROP TRIGGER IF EXISTS afterproductupdate;

DELIMITER $$

CREATE TRIGGER afterproductupdate
AFTER UPDATE ON product
FOR EACH ROW
BEGIN
    -- Insert into history when relevant fields change
    IF OLD.name <> NEW.name 
       OR COALESCE(OLD.stock, 0) <> COALESCE(NEW.stock, 0)
       OR COALESCE(OLD.sell_price, 0) <> COALESCE(NEW.sell_price, 0)
       OR COALESCE(OLD.purchase_price, 0) <> COALESCE(NEW.purchase_price, 0) THEN
        
        INSERT INTO product_history (
            id_product,
            name,
            id_product_type,
            stock,
            sell_price,
            purchase_price,
            date,
            createdBy,
            createdAt
        ) VALUES (
            NEW.id_product,
            NEW.name,
            NEW.id_product_type,
            NEW.stock,
            NEW.sell_price,
            NEW.purchase_price,
            NOW(),
            COALESCE(@app_user, CURRENT_USER()),
            COALESCE(@app_timestamp, NOW())
        );
    END IF;
END$$

DELIMITER ;

-- =========================================
-- 7. Create aftertankupdate trigger
-- =========================================

DROP TRIGGER IF EXISTS aftertankupdate;

DELIMITER $$

CREATE TRIGGER aftertankupdate
AFTER UPDATE ON tank
FOR EACH ROW
BEGIN
    -- Insert into history when relevant fields change
    IF OLD.number <> NEW.number 
       OR OLD.ability <> NEW.ability 
       OR OLD.compartment <> NEW.compartment 
       OR OLD.stock <> NEW.stock THEN
        
        INSERT INTO tank_history (
            id_tank,
            number,
            ability,
            compartment,
            stock,
            date,
            createdBy,
            createdAt
        ) VALUES (
            NEW.id_tank,
            NEW.number,
            NEW.ability,
            NEW.compartment,
            NEW.stock,
            NOW(),
            COALESCE(@app_user, CURRENT_USER()),
            COALESCE(@app_timestamp, NOW())
        );
    END IF;
END$$

DELIMITER ;

-- =========================================
-- Verification queries (optional - can be commented out in production)
-- =========================================

-- Show hose_history structure
SELECT 'hose_history table structure:' AS info;
DESCRIBE hose_history;

-- Show product_history structure
SELECT 'product_history table structure:' AS info;
DESCRIBE product_history;

-- Show tank_history structure
SELECT 'tank_history table structure:' AS info;
DESCRIBE tank_history;

-- Show all triggers
SELECT 'Active triggers:' AS info;
SHOW TRIGGERS WHERE `Trigger` IN ('trg_insert_hose_history', 'afterhoseupdate', 'afterproductupdate', 'aftertankupdate');

SELECT '✅ Migration completed successfully!' AS message;
