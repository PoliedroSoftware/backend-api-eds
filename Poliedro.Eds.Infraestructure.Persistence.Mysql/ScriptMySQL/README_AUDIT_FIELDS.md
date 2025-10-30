# Audit Fields Migration for hose_history Table

## Overview
This migration adds audit fields (`createdBy`, `createdAt`, `updatedBy`, `updatedAt`) to the `hose_history` table and updates the trigger to capture the actual application user instead of the database user.

## Problem
Previously, when the trigger `trg_insert_hose_history` inserted records into the `hose_history` table, the `createdBy` field (if it existed) would show the database user (e.g., `root@%` or `trigger`) instead of the actual application user who performed the action.

## Solution
1. **Added audit columns** to the `hose_history` table
2. **Updated the trigger** to read the application user from a MySQL session variable `@app_user`
3. **Modified the application code** to set the `@app_user` session variable before database operations
4. **Updated Entity Framework configuration** to map the audit fields

## Migration Steps

### For Existing Databases
Run the migration script to update your existing database:

```bash
mysql -u [username] -p [database_name] < add_audit_fields_to_hose_history.sql
```

Or execute the script manually in your MySQL client.

### For New Installations
The updated `DatabaseDump.sql` already includes the audit fields in the table definition and the updated trigger. Simply run the dump to create the database structure.

## How It Works

### Application Side (C# Code)
The `AuditableDbContext` class has been modified to set the MySQL session variable before saving changes:

```csharp
if (!string.IsNullOrEmpty(currentUser))
{
    await Database.ExecuteSqlRawAsync($"SET @app_user = '{currentUser.Replace("'", "''")}'", cancellationToken);
}
```

This sets the `@app_user` variable with the current application user's identifier.

### Database Side (MySQL Trigger)
The trigger reads from the session variable:

```sql
SET v_current_user = IFNULL(@app_user, CURRENT_USER());
```

If `@app_user` is set by the application, it uses that value. Otherwise, it falls back to `CURRENT_USER()` for compatibility.

## Testing
After applying the migration:

1. Insert a record into `court_dispensers` table through the application
2. Check the `hose_history` table
3. Verify that `createdBy` contains the application user (e.g., `userQA_123`) instead of `root@%` or `trigger`

Example query:
```sql
SELECT id_hose_hose_history, id_hose, createdBy, createdAt 
FROM hose_history 
ORDER BY createdAt DESC 
LIMIT 10;
```

## Rollback
If you need to rollback this migration, you can:

1. Drop the audit columns:
```sql
ALTER TABLE hose_history
DROP COLUMN createdBy,
DROP COLUMN createdAt,
DROP COLUMN updatedBy,
DROP COLUMN updatedAt;
```

2. Restore the original trigger (backup recommended before migration)

## Future Considerations
The issue mentioned `product_history` and `tank_history` tables, but these don't currently exist in the codebase. If they are created in the future, apply the same pattern:
- Add audit columns to the table
- Update the trigger to use `@app_user`
- Ensure the entity extends `AuditableEntity`
- Configure column mappings in Entity Framework

## Related Files
- `add_audit_fields_to_hose_history.sql` - Migration script
- `DatabaseDump.sql` - Updated database dump
- `AuditableDbContext.cs` - Sets the session variable
- `HoseHistoryConfiguration.cs` - Entity Framework configuration
- `HoseHistoryEntity.cs` - Extends `AuditableEntity`
