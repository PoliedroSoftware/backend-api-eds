-- Migration script to rename type_of_collection to payment_method
-- This script renames all tables, columns, constraints, indexes, and views
-- related to type_of_collection to use the new name payment_method

-- Step 1: Drop views that depend on the tables we're going to rename
DROP VIEW IF EXISTS `v_court_collection`;
DROP VIEW IF EXISTS `v_type_of_collection`;

-- Step 2: Drop foreign key constraints in the pivot table
ALTER TABLE `court_type_of_collection` 
  DROP FOREIGN KEY `fk_court_has_type_of_collection_type_of_collection1`,
  DROP FOREIGN KEY `fk_court_has_type_of_collection_court1`;

-- Step 3: Rename the main table from type_of_collection to payment_method
RENAME TABLE `type_of_collection` TO `payment_method`;

-- Step 4: Rename the column in payment_method table
ALTER TABLE `payment_method` 
  CHANGE COLUMN `id_type_of_collection` `id_payment_method` INT NOT NULL AUTO_INCREMENT;

-- Step 5: Rename the pivot table from court_type_of_collection to court_payment_method
RENAME TABLE `court_type_of_collection` TO `court_payment_method`;

-- Step 6: Rename columns in the pivot table
ALTER TABLE `court_payment_method` 
  CHANGE COLUMN `id_court_type_of_collection` `id_court_payment_method` INT NOT NULL AUTO_INCREMENT,
  CHANGE COLUMN `id_type_of_collection` `id_payment_method` INT NOT NULL;

-- Step 7: Drop old indexes from the pivot table
ALTER TABLE `court_payment_method` 
  DROP INDEX `fk_court_has_type_of_collection_type_of_collection1_idx`,
  DROP INDEX `fk_court_has_type_of_collection_court1_idx`;

-- Step 8: Add new indexes with updated names
ALTER TABLE `court_payment_method` 
  ADD INDEX `fk_court_has_payment_method_payment_method1_idx` (`id_payment_method` ASC),
  ADD INDEX `fk_court_has_payment_method_court1_idx` (`id_court` ASC);

-- Step 9: Add foreign key constraints with new names
ALTER TABLE `court_payment_method` 
  ADD CONSTRAINT `fk_court_has_payment_method_court1` 
    FOREIGN KEY (`id_court`) 
    REFERENCES `court` (`id_court`),
  ADD CONSTRAINT `fk_court_has_payment_method_payment_method1` 
    FOREIGN KEY (`id_payment_method`) 
    REFERENCES `payment_method` (`id_payment_method`);

-- Step 10: Recreate v_court_collection view with new table and column names
CREATE 
ALGORITHM=UNDEFINED 
DEFINER=`root`@`%` 
SQL SECURITY DEFINER
VIEW `v_court_collection` AS 
SELECT 
  `cc`.`id_court_payment_method` AS `id`,
  `c`.`id_court` AS `court`,
  `c`.`date_starttime` AS `date`,
  `pm`.`description` AS `collection`,
  `cc`.`amount` AS `amount`,
  `cc`.`description` AS `description` 
FROM ((`court_payment_method` `cc` 
  LEFT JOIN `court` `c` ON((`c`.`id_court` = `cc`.`id_court`))) 
  LEFT JOIN `payment_method` `pm` ON((`pm`.`id_payment_method` = `cc`.`id_payment_method`))) 
ORDER BY `c`.`id_court` DESC;

-- Step 11: Recreate v_type_of_collection view as v_payment_method with new table and column names
CREATE 
ALGORITHM=UNDEFINED 
DEFINER=`root`@`%` 
SQL SECURITY DEFINER
VIEW `v_payment_method` AS 
SELECT 
  `pm`.`id_payment_method` AS `id_payment_method`,
  `pm`.`description` AS `description`,
  `p`.`id_product` AS `id_product`,
  `b`.`id_business` AS `id_business`,
  `p`.`date` AS `date` 
FROM ((`payment_method` `pm` 
  LEFT JOIN `business` `b` ON((`b`.`id_business` = `pm`.`id_payment_method`))) 
  LEFT JOIN `product` `p` ON((`p`.`id_product` = `pm`.`id_payment_method`)));

-- Migration completed successfully
