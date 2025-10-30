-- Migration: Add audit fields to hose_history table and update trigger
-- Date: 2025-10-30
-- Purpose: Fix audit trail by capturing actual application user instead of database user

-- Step 1: Add audit columns to hose_history table
ALTER TABLE `hose_history`
ADD COLUMN `createdBy` VARCHAR(255) NULL AFTER `date`,
ADD COLUMN `createdAt` DATETIME NULL AFTER `createdBy`,
ADD COLUMN `updatedBy` VARCHAR(255) NULL AFTER `createdAt`,
ADD COLUMN `updatedAt` DATETIME NULL AFTER `updatedBy`;

-- Step 2: Drop the existing trigger
DROP TRIGGER IF EXISTS `trg_insert_hose_history`;

-- Step 3: Recreate the trigger with audit field support
DELIMITER $$

CREATE TRIGGER `trg_insert_hose_history` 
AFTER INSERT ON `court_dispensers` 
FOR EACH ROW 
BEGIN
    DECLARE court_date DATE;
    DECLARE v_id_dispensers INT;
    DECLARE v_current_user VARCHAR(255);
    DECLARE v_current_datetime DATETIME;

    -- Get the date from court table
    SELECT date_starttime INTO court_date
    FROM court
    WHERE id_court = NEW.id_court;

    -- Get the dispenser id from hose table
    SELECT id_dispensers INTO v_id_dispensers
    FROM hose
    WHERE id_hose = NEW.id_hose;

    -- Get the application user from session variable (set by application)
    -- If not set, fall back to CURRENT_USER()
    SET v_current_user = IFNULL(@app_user, CURRENT_USER());
    
    -- Get current timestamp
    SET v_current_datetime = NOW();

    -- Insert into hose_history with audit fields
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
