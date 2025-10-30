-- Migration Script: Make invoice column nullable in shopping table
-- Date: 2025-10-30
-- Description: Allow purchases without invoice number when provider is "Otros"

-- Make the invoice column nullable
ALTER TABLE `shopping` 
MODIFY COLUMN `invoice` VARCHAR(45) NULL;

-- Note: This change allows the system to create shopping records without an invoice number
-- when the provider is set to "Otros" (Others), enabling informal purchases or purchases
-- without receipts to be recorded in the system.
