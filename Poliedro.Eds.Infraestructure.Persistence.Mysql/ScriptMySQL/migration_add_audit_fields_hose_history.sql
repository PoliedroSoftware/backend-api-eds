-- Migration script to add audit fields to hose_history table
-- This script should be executed on existing databases (main and QA)
-- to add the audit fields that are now part of the table structure

-- Add audit fields to hose_history table if they don't exist
ALTER TABLE `hose_history`
ADD COLUMN IF NOT EXISTS `createdBy` varchar(255) DEFAULT NULL AFTER `date`,
ADD COLUMN IF NOT EXISTS `createdAt` datetime DEFAULT NULL AFTER `createdBy`,
ADD COLUMN IF NOT EXISTS `updatedBy` varchar(255) DEFAULT NULL AFTER `createdAt`,
ADD COLUMN IF NOT EXISTS `updatedAt` datetime DEFAULT NULL AFTER `updatedBy`;

-- Update the trigger to populate audit fields
DROP TRIGGER IF EXISTS `trg_insert_hose_history`;

DELIMITER $$
CREATE TRIGGER `trg_insert_hose_history` AFTER INSERT ON `court_dispensers` FOR EACH ROW 
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
        'trigger',
        CONVERT_TZ(NOW(), 'UTC', 'America/Bogota')
    );
END$$
DELIMITER ;
