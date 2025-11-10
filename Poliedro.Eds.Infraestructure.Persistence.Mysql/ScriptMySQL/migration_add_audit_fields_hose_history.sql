-- Migration script to add audit fields to hose and hose_history tables
-- This script should be executed on existing databases (main and QA)
-- to add the audit fields that are now part of the table structure

-- Remove the 'date' column from hose_history if it exists (it should not be there)
ALTER TABLE `hose_history`
DROP COLUMN IF EXISTS `date`;

-- Add audit fields to hose_history table if they don't exist
ALTER TABLE `hose_history`
ADD COLUMN IF NOT EXISTS `createdBy` varchar(255) DEFAULT NULL,
ADD COLUMN IF NOT EXISTS `createdAt` datetime DEFAULT NULL,
ADD COLUMN IF NOT EXISTS `updatedBy` varchar(255) DEFAULT NULL,
ADD COLUMN IF NOT EXISTS `updatedAt` datetime DEFAULT NULL;

-- Add audit fields to hose table if they don't exist
ALTER TABLE `hose`
ADD COLUMN IF NOT EXISTS `id_compartiment` int DEFAULT NULL,
ADD COLUMN IF NOT EXISTS `createdBy` varchar(255) DEFAULT NULL,
ADD COLUMN IF NOT EXISTS `createdAt` datetime DEFAULT NULL,
ADD COLUMN IF NOT EXISTS `updatedBy` varchar(255) DEFAULT NULL,
ADD COLUMN IF NOT EXISTS `updatedAt` datetime DEFAULT NULL;

-- Remove old incorrect triggers if they exist
DROP TRIGGER IF EXISTS `trg_insert_hose_history`;
DROP TRIGGER IF EXISTS `trg_update_hose_history`;
DROP TRIGGER IF EXISTS `trg_update_hose_accumulated_on_update`;

-- Create the correct trigger on hose table to track changes in hose_history
DROP TRIGGER IF EXISTS `after_hose_update`;

DELIMITER $$
CREATE TRIGGER `after_hose_update` AFTER UPDATE ON `hose` FOR EACH ROW 
BEGIN
    -- Only save to history if accumulated_amount changed
    IF (OLD.accumulated_amount <> NEW.accumulated_amount) THEN
        INSERT INTO hose_history (
            id_hose,
            id_dispensers,
            accumulated_amount,
            accumulated_gallons,
            createdBy,
            createdAt
        ) VALUES (
            OLD.id_hose,
            OLD.id_dispensers,
            NEW.accumulated_amount,
            NEW.accumulated_gallons,
            CURRENT_USER(),
            CONVERT_TZ(NOW(), 'UTC', 'America/Bogota')
        );
    END IF;
END$$
DELIMITER ;
