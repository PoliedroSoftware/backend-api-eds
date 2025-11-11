-- Test script to verify audit fields implementation
-- This script demonstrates how the audit fields work with session variables
-- Run this script in DBeaver or MySQL Workbench after applying the migration

USE eds_new;

-- =========================================
-- SETUP: Display current state
-- =========================================

SELECT '=== INITIAL STATE ===' AS TestPhase;

-- Show existing triggers
SELECT 'Active Triggers:' AS Info;
SHOW TRIGGERS WHERE `Trigger` IN ('trg_insert_hose_history', 'afterhoseupdate', 'afterproductupdate', 'aftertankupdate');

-- Check table structures
SELECT 'hose_history structure:' AS Info;
SHOW COLUMNS FROM hose_history WHERE Field IN ('createdBy', 'createdAt', 'updatedBy', 'updatedAt');

SELECT 'product_history exists:' AS Info;
SELECT COUNT(*) AS table_exists FROM information_schema.tables 
WHERE table_schema = DATABASE() AND table_name = 'product_history';

SELECT 'tank_history exists:' AS Info;
SELECT COUNT(*) AS table_exists FROM information_schema.tables 
WHERE table_schema = DATABASE() AND table_name = 'tank_history';

-- =========================================
-- TEST 1: Hose Update with Application User
-- =========================================

SELECT '=== TEST 1: HOSE UPDATE ===' AS TestPhase;

-- Simulate application setting session variables
SET @app_user = 'test_user_QA_123';
SET @app_timestamp = '2025-01-15 14:30:45';

-- Get current values before update
SELECT 'Before Update:' AS Info;
SELECT id_hose, accumulated_amount, accumulated_gallons 
FROM hose 
WHERE id_hose = 1;

-- Perform update (this should trigger afterhoseupdate)
UPDATE hose 
SET accumulated_amount = accumulated_amount + 100.0,
    accumulated_gallons = accumulated_gallons + 10.0
WHERE id_hose = 1;

-- Check the history table
SELECT 'After Update - History Record:' AS Info;
SELECT id_hose, accumulated_amount, accumulated_gallons, createdBy, createdAt
FROM hose_history 
WHERE id_hose = 1 
ORDER BY createdAt DESC 
LIMIT 1;

-- Verify the user is recorded correctly
SELECT 'Verification:' AS Info;
SELECT 
    CASE 
        WHEN createdBy = 'test_user_QA_123' THEN '✅ PASS: createdBy is correct'
        ELSE '❌ FAIL: createdBy is incorrect'
    END AS createdBy_check,
    CASE 
        WHEN createdAt = '2025-01-15 14:30:45' THEN '✅ PASS: createdAt is correct'
        ELSE '❌ FAIL: createdAt is incorrect'
    END AS createdAt_check
FROM hose_history 
WHERE id_hose = 1 
ORDER BY createdAt DESC 
LIMIT 1;

-- =========================================
-- TEST 2: Product Update with Different User
-- =========================================

SELECT '=== TEST 2: PRODUCT UPDATE ===' AS TestPhase;

-- Set different session variables
SET @app_user = 'product_manager_456';
SET @app_timestamp = NOW();

-- Get current values before update
SELECT 'Before Update:' AS Info;
SELECT id_product, name, stock 
FROM product 
WHERE id_product = 1;

-- Perform update (this should trigger afterproductupdate)
UPDATE product 
SET stock = COALESCE(stock, 0) + 50.0
WHERE id_product = 1;

-- Check the history table
SELECT 'After Update - History Record:' AS Info;
SELECT id_product, name, stock, createdBy, createdAt
FROM product_history 
WHERE id_product = 1 
ORDER BY createdAt DESC 
LIMIT 1;

-- Verify the user is recorded correctly
SELECT 'Verification:' AS Info;
SELECT 
    CASE 
        WHEN createdBy = 'product_manager_456' THEN '✅ PASS: createdBy is correct'
        ELSE '❌ FAIL: createdBy is incorrect'
    END AS createdBy_check,
    createdBy,
    createdAt
FROM product_history 
WHERE id_product = 1 
ORDER BY createdAt DESC 
LIMIT 1;

-- =========================================
-- TEST 3: Tank Update
-- =========================================

SELECT '=== TEST 3: TANK UPDATE ===' AS TestPhase;

-- Set session variables
SET @app_user = 'tank_operator_789';
SET @app_timestamp = NOW();

-- Get current values before update
SELECT 'Before Update:' AS Info;
SELECT id_tank, number, stock 
FROM tank 
WHERE id_tank = 1;

-- Perform update (this should trigger aftertankupdate)
UPDATE tank 
SET stock = stock + 100.0
WHERE id_tank = 1;

-- Check the history table
SELECT 'After Update - History Record:' AS Info;
SELECT id_tank, number, stock, createdBy, createdAt
FROM tank_history 
WHERE id_tank = 1 
ORDER BY createdAt DESC 
LIMIT 1;

-- Verify the user is recorded correctly
SELECT 'Verification:' AS Info;
SELECT 
    CASE 
        WHEN createdBy = 'tank_operator_789' THEN '✅ PASS: createdBy is correct'
        ELSE '❌ FAIL: createdBy is incorrect'
    END AS createdBy_check,
    createdBy,
    createdAt
FROM tank_history 
WHERE id_tank = 1 
ORDER BY createdAt DESC 
LIMIT 1;

-- =========================================
-- TEST 4: Fallback to CURRENT_USER (no session variable)
-- =========================================

SELECT '=== TEST 4: FALLBACK TEST ===' AS TestPhase;

-- Clear session variables
SET @app_user = NULL;
SET @app_timestamp = NULL;

-- Perform update
UPDATE hose 
SET accumulated_amount = accumulated_amount + 1.0
WHERE id_hose = 2;

-- Check the history - should show database user
SELECT 'Fallback Test - History Record:' AS Info;
SELECT id_hose, createdBy, createdAt
FROM hose_history 
WHERE id_hose = 2 
ORDER BY createdAt DESC 
LIMIT 1;

SELECT 'Verification:' AS Info;
SELECT 
    CASE 
        WHEN createdBy LIKE '%@%' THEN '✅ PASS: Fallback to CURRENT_USER() works'
        ELSE '❌ FAIL: Fallback not working'
    END AS fallback_check,
    createdBy,
    'Should show database user (e.g., root@%)' AS expected
FROM hose_history 
WHERE id_hose = 2 
ORDER BY createdAt DESC 
LIMIT 1;

-- =========================================
-- SUMMARY
-- =========================================

SELECT '=== TEST SUMMARY ===' AS TestPhase;

SELECT 'Recent hose_history records:' AS Info;
SELECT id_hose, accumulated_amount, createdBy, createdAt
FROM hose_history 
ORDER BY createdAt DESC 
LIMIT 5;

SELECT 'Recent product_history records:' AS Info;
SELECT id_product, name, stock, createdBy, createdAt
FROM product_history 
ORDER BY createdAt DESC 
LIMIT 5;

SELECT 'Recent tank_history records:' AS Info;
SELECT id_tank, number, stock, createdBy, createdAt
FROM tank_history 
ORDER BY createdAt DESC 
LIMIT 5;

SELECT '=== TESTS COMPLETED ===' AS TestPhase;
SELECT 'If all checks show ✅ PASS, the implementation is working correctly!' AS Result;
