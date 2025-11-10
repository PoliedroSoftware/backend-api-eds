# Critical: Deployment Instructions for History Triggers Fix

## Current Issue
QA is still seeing the old behavior because **the migration script has NOT been deployed** to the QA database. The triggers in QA are still using:
- `CURRENT_USER()` or `USER()` instead of `@app_user` 
- `NOW()` instead of `CONVERT_TZ(NOW(), '+00:00', '-05:00')`

## Evidence from QA
- `hose_history.createdBy` shows `root@%` 
- `product_history.createdBy` shows `root@201.244.214.101`
- Timestamps are 5 hours ahead (showing UTC instead of Colombia time)

## Root Cause
The application code (AuditableDbContext.cs) IS setting the `@app_user` session variable correctly, but the database triggers have NOT been updated to read from it.

## Deployment Steps

### Option 1: Full Verification and Deployment (Recommended)
This script will show you the current state before making changes:

```bash
mysql -u username -p database_name < verify_and_deploy_triggers.sql > deployment_log.txt 2>&1
```

Review `deployment_log.txt` to see:
1. Current trigger definitions (backup)
2. Current history table entries
3. Deployment results
4. New trigger definitions (verification)

### Option 2: Direct Deployment
If you trust the script and want to deploy directly:

```bash
mysql -u username -p database_name < fix_history_triggers_v2.sql
```

### Option 3: Manual Step-by-Step
```sql
-- 1. Connect to database
mysql -u username -p database_name

-- 2. Check current triggers
SHOW CREATE TRIGGER after_hose_update;
SHOW CREATE TRIGGER after_product_update;
SHOW CREATE TRIGGER after_tank_update;

-- 3. Drop old triggers
DROP TRIGGER IF EXISTS after_hose_update;
DROP TRIGGER IF EXISTS after_product_update;
DROP TRIGGER IF EXISTS after_tank_update;

-- 4. Apply the fix_history_triggers_v2.sql script
SOURCE /path/to/fix_history_triggers_v2.sql;

-- 5. Verify new triggers
SHOW CREATE TRIGGER after_hose_update;
```

## Post-Deployment Verification

### Step 1: Verify Triggers Were Updated
```sql
SELECT 
    TRIGGER_NAME, 
    ACTION_STATEMENT
FROM information_schema.TRIGGERS
WHERE TRIGGER_SCHEMA = DATABASE()
  AND EVENT_OBJECT_TABLE IN ('hose', 'product', 'tank');
```

Look for `@app_user` and `CONVERT_TZ` in the ACTION_STATEMENT.

### Step 2: Test with Application
1. **Login to the application** as a known user (e.g., "admin")
2. **Update a hose record** through the application
3. **Check hose_history**:
```sql
SELECT id_hose, createdBy, createdAt
FROM hose_history
ORDER BY id_hose_hose_history DESC
LIMIT 5;
```

Expected results:
- `createdBy` should show "admin" (NOT "root@%" or "root@IP")
- `createdAt` should be in Colombia time (5 hours behind UTC)

### Step 3: Verify Timezone
```sql
SELECT 
    NOW() AS utc_time,
    CONVERT_TZ(NOW(), '+00:00', '-05:00') AS colombia_time,
    TIMESTAMPDIFF(HOUR, CONVERT_TZ(NOW(), '+00:00', '-05:00'), NOW()) AS should_be_5;
```

The `should_be_5` column should show 5.

## Troubleshooting

### Issue: Still seeing root@% after deployment
**Cause**: Application might not be setting @app_user
**Solution**: 
1. Verify application code has the fix in AuditableDbContext.cs
2. Restart the application to load new code
3. Check application logs for any errors setting the session variable

### Issue: Timezone still wrong
**Cause**: CONVERT_TZ might not be working on your MySQL server
**Solution**:
```sql
-- Check if timezone data is loaded
SELECT CONVERT_TZ('2025-01-01 12:00:00', '+00:00', '-05:00');
-- If returns NULL, timezone data needs to be loaded
```

If NULL, you need to load timezone data:
```bash
mysql_tzinfo_to_sql /usr/share/zoneinfo | mysql -u root -p mysql
```

### Issue: Tank updates still return 500 error
**Cause**: TankEntity had `init` properties that prevented updates
**Solution**: This is fixed in the latest code (commit in progress). Deploy the updated application code.

## Checklist for QA Team

- [ ] Backup current database triggers (optional but recommended)
- [ ] Deploy `verify_and_deploy_triggers.sql` OR `fix_history_triggers_v2.sql`
- [ ] Verify triggers were created successfully
- [ ] Deploy latest application code (for tank fix)
- [ ] Restart application
- [ ] Test hose update → Check hose_history for correct user and time
- [ ] Test product update → Check product_history for correct user and time  
- [ ] Test tank update → Check tank_history for correct user and time
- [ ] Verify no 500 errors on tank operations

## Expected Results After Deployment

| Table | Field | Before | After |
|-------|-------|--------|-------|
| hose_history | createdBy | root@% | admin |
| hose_history | createdAt | 2025-11-01 05:00:00 | 2025-11-01 00:00:00 |
| product_history | createdBy | root@201.244.214.101 | admin |
| product_history | createdAt | 2025-11-01 05:00:00 | 2025-11-01 00:00:00 |
| tank_history | createdBy | root@... | admin |
| tank_history | createdAt | 2025-11-01 05:00:00 | 2025-11-01 00:00:00 |

## Files Involved

1. **fix_history_triggers_v2.sql** - Main migration script
2. **verify_and_deploy_triggers.sql** - Migration with verification
3. **AuditableDbContext.cs** - Sets @app_user (already deployed)
4. **TankEntity.cs** - Fixed init properties (needs deployment)

## Contact
If issues persist after deployment, provide:
1. Output of `verify_and_deploy_triggers.sql`
2. Recent entries from history tables
3. Application logs during update operation
