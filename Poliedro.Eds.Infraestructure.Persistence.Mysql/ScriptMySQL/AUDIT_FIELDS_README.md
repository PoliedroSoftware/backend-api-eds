# Audit Fields Implementation for History Tables

## Overview

This implementation fixes the issue where history tables (hose_history, product_history, tank_history) were recording the database user (e.g., `root@%`, `trigger`) instead of the actual application user in the `createdBy` field.

## Solution Summary

The solution uses MySQL session variables (`@app_user`, `@app_timestamp`) that are set by the application before each database operation. The triggers then use these variables via `COALESCE(@app_user, CURRENT_USER())` to record the correct user information.

## Changes Made

### 1. Database Changes (`audit_triggers_migration.sql`)

#### Added Audit Fields to hose_history Table
- `createdBy VARCHAR(255)` - Application user who created the record
- `createdAt DATETIME` - Local Colombia timestamp when created
- `updatedBy VARCHAR(255)` - Application user who last updated
- `updatedAt DATETIME` - Local Colombia timestamp when updated

#### Created product_history Table
```sql
CREATE TABLE product_history (
    id_product_history INT PRIMARY KEY AUTO_INCREMENT,
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
    FOREIGN KEY (id_product) REFERENCES product(id_product)
);
```

#### Created tank_history Table
```sql
CREATE TABLE tank_history (
    id_tank_history INT PRIMARY KEY AUTO_INCREMENT,
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
    FOREIGN KEY (id_tank) REFERENCES tank(id_tank)
);
```

#### Updated/Created Triggers

1. **trg_insert_hose_history** - Updated existing trigger
   - Triggered: AFTER INSERT ON court_dispensers
   - Records hose history with proper audit fields

2. **afterhoseupdate** - New trigger
   - Triggered: AFTER UPDATE ON hose
   - Records history when accumulated_amount or accumulated_gallons change

3. **afterproductupdate** - New trigger
   - Triggered: AFTER UPDATE ON product
   - Records history when name, stock, sell_price, or purchase_price change

4. **aftertankupdate** - New trigger
   - Triggered: AFTER UPDATE ON tank
   - Records history when number, ability, compartment, or stock change

All triggers use:
```sql
createdBy = COALESCE(@app_user, CURRENT_USER())
createdAt = COALESCE(@app_timestamp, NOW())
```

### 2. Application Changes

#### Modified: `AuditableDbContext.cs`

Enhanced the `SaveChangesAsync` method to set MySQL session variables before saving changes:

```csharp
// Set MySQL session variables for triggers to use
if (!string.IsNullOrEmpty(currentUser))
{
    await Database.ExecuteSqlRawAsync(
        "SET @app_user = {0}, @app_timestamp = {1}",
        currentUser,
        currentTimestamp.ToString("yyyy-MM-dd HH:mm:ss")
    );
}
```

**Key points:**
- Gets current user from `httpContextAccessor.HttpContext.Items["identifiername"]`
- Uses `DateTimeColombiaHelper.NowColombia()` for local Colombia time
- Sets session variables before each SaveChangesAsync operation
- Maintains existing audit field population for Entity Framework entities

## How It Works

### Flow Diagram

```
Application Request
    ↓
HttpContext contains user info
    ↓
AuditableDbContext.SaveChangesAsync()
    ↓
1. Extract currentUser from HttpContext
2. Get currentTimestamp (Colombia time)
3. Update AuditableEntity fields (CreatedBy, CreatedAt, etc.)
4. Execute SQL: SET @app_user='user', @app_timestamp='2025-01-15 10:30:00'
5. Call base.SaveChangesAsync()
    ↓
Database Operation (INSERT/UPDATE)
    ↓
Trigger Fires (afterhoseupdate, afterproductupdate, aftertankupdate)
    ↓
Trigger uses COALESCE(@app_user, CURRENT_USER())
    ↓
History table receives correct user and timestamp
```

## Deployment Instructions

### 1. Apply Database Migration

Execute the migration script on your MySQL database:

```bash
mysql -u [username] -p [database_name] < audit_triggers_migration.sql
```

Or in DBeaver/MySQL Workbench:
1. Open `audit_triggers_migration.sql`
2. Ensure you're connected to the correct database (eds_new)
3. Execute the entire script

### 2. Deploy Application

The application changes are in `AuditableDbContext.cs`. After deployment:
1. Restart the application
2. The changes will automatically take effect

## Testing Instructions

### Manual Testing in DBeaver

1. **Simulate Application Context:**
```sql
-- Set session variables as the application would
SET @app_user = 'test_user_123';
SET @app_timestamp = '2025-01-15 14:30:00';

-- Update a hose record
UPDATE hose SET accumulated_amount = 1500.0 WHERE id_hose = 1;

-- Verify the history
SELECT * FROM hose_history WHERE id_hose = 1 ORDER BY createdAt DESC LIMIT 1;
-- Should show createdBy = 'test_user_123' and createdAt = '2025-01-15 14:30:00'
```

2. **Test Product History:**
```sql
SET @app_user = 'product_admin';
SET @app_timestamp = NOW();

UPDATE product SET stock = 100.0 WHERE id_product = 1;

SELECT * FROM product_history WHERE id_product = 1 ORDER BY createdAt DESC LIMIT 1;
```

3. **Test Tank History:**
```sql
SET @app_user = 'tank_operator';
SET @app_timestamp = NOW();

UPDATE tank SET stock = 5000.0 WHERE id_tank = 1;

SELECT * FROM tank_history WHERE id_tank = 1 ORDER BY createdAt DESC LIMIT 1;
```

### Application Testing

1. **Login to the application** with a known user
2. **Update a hose, product, or tank** through the API
3. **Query the corresponding history table** and verify:
   - `createdBy` contains the application username (not `root@%` or `trigger`)
   - `createdAt` contains the Colombia local time

### Verification Queries

```sql
-- Check hose_history
SELECT id_hose, accumulated_amount, createdBy, createdAt 
FROM hose_history 
ORDER BY createdAt DESC LIMIT 10;

-- Check product_history  
SELECT id_product, name, stock, createdBy, createdAt 
FROM product_history 
ORDER BY createdAt DESC LIMIT 10;

-- Check tank_history
SELECT id_tank, number, stock, createdBy, createdAt 
FROM tank_history 
ORDER BY createdAt DESC LIMIT 10;
```

## Rollback Instructions

If you need to rollback the changes:

```sql
-- Remove triggers
DROP TRIGGER IF EXISTS afterhoseupdate;
DROP TRIGGER IF EXISTS afterproductupdate;
DROP TRIGGER IF EXISTS aftertankupdate;

-- Remove audit columns from hose_history
ALTER TABLE hose_history 
    DROP COLUMN createdBy,
    DROP COLUMN createdAt,
    DROP COLUMN updatedBy,
    DROP COLUMN updatedAt;

-- Drop new tables
DROP TABLE IF EXISTS product_history;
DROP TABLE IF EXISTS tank_history;

-- Restore original trg_insert_hose_history (see original DatabaseDump.sql)
```

Then redeploy the previous version of the application.

## Benefits

1. **Improved Traceability**: Know exactly which application user made changes
2. **Better Auditing**: Accurate timestamp in local Colombia time
3. **Compliance**: Meets audit and regulatory requirements
4. **Debugging**: Easier to track down who made problematic changes
5. **Minimal Impact**: Changes are transparent to the rest of the application

## Technical Notes

- Session variables are connection-scoped and don't persist across connections
- The COALESCE ensures fallback to CURRENT_USER() if session variable not set
- Colombia timezone is handled consistently by DateTimeColombiaHelper
- The solution works with Entity Framework's existing audit mechanism
- No changes required to existing API controllers or business logic

## Maintenance

- When adding new history tables, follow the same pattern:
  1. Add createdBy, createdAt, updatedBy, updatedAt columns
  2. Create trigger with COALESCE(@app_user, CURRENT_USER())
  3. No application code changes needed (AuditableDbContext handles it)

## Support

For issues or questions:
1. Check trigger status: `SHOW TRIGGERS WHERE Trigger LIKE 'after%';`
2. Verify session variables work: `SET @app_user='test'; SELECT @app_user;`
3. Check application logs for any database errors
4. Verify HttpContext contains 'identifiername' item
