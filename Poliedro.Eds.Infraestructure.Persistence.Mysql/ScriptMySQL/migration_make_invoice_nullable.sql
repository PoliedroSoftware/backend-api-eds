-- Migration Script: Make invoice column nullable in shopping table
-- Date: 2024-10-30
-- Description: Allow purchases without invoice number when provider is "Otros"

-- Make the invoice column nullable
ALTER TABLE `shopping` 
MODIFY COLUMN `invoice` VARCHAR(45) NULL;

-- Note: This change allows the system to create shopping records without an invoice number
-- when the provider is set to "Otros" (Others), enabling informal purchases or purchases
-- without receipts to be recorded in the system.

-- ========================================
-- ROLLBACK INSTRUCTIONS
-- ========================================
-- To revert this migration (if needed):
-- 
-- WARNING: This rollback will FAIL if there are existing NULL values in the invoice column.
-- Before running the rollback, you must update all NULL invoice values to a default value.
--
-- Step 1: Update NULL values (run this first if there are NULL invoices)
-- UPDATE `shopping` SET `invoice` = 'N/A' WHERE `invoice` IS NULL;
--
-- Step 2: Revert the column to NOT NULL
-- ALTER TABLE `shopping` 
-- MODIFY COLUMN `invoice` VARCHAR(45) NOT NULL;
