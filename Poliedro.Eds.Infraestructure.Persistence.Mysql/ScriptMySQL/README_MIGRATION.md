# Database Migration: Rename type_of_collection to payment_method

## Overview
This migration renames all database objects related to `type_of_collection` to use the new name `payment_method`. This change improves the coherence of the schema by using terminology aligned with payment/finance operations.

## Migration Script
File: `migration_rename_type_of_collection_to_payment_method.sql`

## What Gets Changed

### Tables
- `type_of_collection` → `payment_method`
- `court_type_of_collection` → `court_payment_method`

### Columns
- `id_type_of_collection` → `id_payment_method`
- `id_court_type_of_collection` → `id_court_payment_method`

### Constraints and Indexes
All foreign key constraints and indexes are updated with the new naming convention:
- `fk_court_has_type_of_collection_*` → `fk_court_has_payment_method_*`
- Index names updated accordingly

### Views
- `v_type_of_collection` → `v_payment_method`
- `v_court_collection` (updated to reference new tables and columns)

## How to Execute the Migration

### Prerequisites
- Backup your database before running the migration
- Ensure no active connections are using the affected tables
- Have appropriate MySQL privileges (ALTER, DROP, CREATE permissions)

### Execution Steps

1. **Backup the database:**
   ```bash
   mysqldump -u [username] -p [database_name] > backup_before_migration.sql
   ```

2. **Run the migration script:**
   ```bash
   mysql -u [username] -p [database_name] < migration_rename_type_of_collection_to_payment_method.sql
   ```

3. **Verify the migration:**
   ```sql
   -- Check that new tables exist
   SHOW TABLES LIKE '%payment_method%';
   
   -- Check that old tables don't exist
   SHOW TABLES LIKE '%type_of_collection%';
   
   -- Verify views
   SELECT * FROM v_payment_method LIMIT 1;
   SELECT * FROM v_court_collection LIMIT 1;
   ```

## Rollback Plan

If you need to rollback the migration, you can restore from the backup:

```bash
mysql -u [username] -p [database_name] < backup_before_migration.sql
```

## Post-Migration Tasks

After running this migration, you should also update:
1. Application code that references the old table/column names
2. ORM/Entity Framework configurations
3. Any stored procedures or triggers (if applicable)
4. API documentation
5. Integration tests

## Database Compatibility
- Tested for: MySQL 5.7+, MySQL 8.0+
- Compatible with: MariaDB 10.2+

## Notes
- The migration is designed to be run as a single transaction where possible
- Views are dropped and recreated to avoid dependency issues
- All foreign key constraints are properly maintained
- The migration preserves all existing data
