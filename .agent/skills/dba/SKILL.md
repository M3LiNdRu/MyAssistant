---
name: dba
description: MongoDB database administrator for querying and inspecting the MyAssistant expenses database. Use when the user wants to query, explore, or analyze data in MongoDB, inspect collections, run aggregations, or troubleshoot data issues.
---

# MongoDB DBA Skill

## Prerequisites

This skill requires `mongosh` (MongoDB Shell) to be installed and available on PATH.

## Connection Details

Connection settings are stored in the .NET API configuration:

1. **Check `appsettings.json`** at `src/expenses-api/Expenses.Api/src/appsettings.json` for the `MongoSettings` section:
   - `MongoSettings:ConnectionString` — the MongoDB connection URI
   - `MongoSettings:Database` — the database name
2. **User secrets** may override these values. Check with:
   ```bash
   cd src/expenses-api/Expenses.Api/src && dotnet user-secrets list
   ```
3. If neither source has credentials, ask the user for the connection string.

## Connecting

```bash
mongosh "<ConnectionString>" --eval '<query>'
# Or for interactive exploration:
mongosh "<ConnectionString>"
```

Always target the correct database:

```bash
mongosh "<ConnectionString>" --eval 'use("<Database>"); <query>'
```

## Database Schema

Refer to `references/SCHEMA.md` for detailed collection schemas. Summary:

| Collection     | Description                        |
|----------------|------------------------------------|
| `Expenses`     | Individual expense records         |
| `Categories`   | Expense category definitions       |
| `Tags`         | Single document holding all tag names |

All field names are **camelCase** in MongoDB (via `CamelCaseElementNameConvention`). The `_id` fields are stored as ObjectId.

## Common Queries

### List recent expenses
```javascript
db.Expenses.find().sort({ timestamp: -1 }).limit(10)
```

### Expenses for a specific month
```javascript
db.Expenses.find({
  timestamp: {
    $gte: ISODate("2026-01-01"),
    $lt: ISODate("2026-02-01")
  }
}).sort({ timestamp: -1 })
```

### Aggregate total by category
```javascript
db.Expenses.aggregate([
  { $group: { _id: "$category", total: { $sum: "$amount" } } },
  { $sort: { total: -1 } }
])
```

### Monthly spending totals
```javascript
db.Expenses.aggregate([
  { $group: {
    _id: { year: { $year: "$timestamp" }, month: { $month: "$timestamp" } },
    total: { $sum: "$amount" },
    count: { $sum: 1 }
  }},
  { $sort: { "_id.year": -1, "_id.month": -1 } }
])
```

### Search expenses by tag
```javascript
db.Expenses.find({ tags: "groceries" })
```

### List all categories
```javascript
db.Categories.find()
```

### List all tags
```javascript
db.Tags.find()
```

### Count expenses per category
```javascript
db.Expenses.aggregate([
  { $group: { _id: "$category", count: { $sum: 1 } } },
  { $sort: { count: -1 } }
])
```

### Find top expenses by amount
```javascript
db.Expenses.find().sort({ amount: -1 }).limit(10)
```

## Safety Rules

1. **Read-only by default** — Only execute read operations (`find`, `aggregate`, `count`, `distinct`) unless the user explicitly requests a mutation.
2. **Mutations require confirmation** — Before running any `insert`, `update`, `delete`, `drop`, or `replaceOne` operation, show the user the exact command and ask for explicit confirmation.
3. **Never drop collections or databases** without the user typing the exact confirmation.
4. **Always limit result sets** — Use `.limit()` on queries that could return large datasets to avoid overwhelming output.
5. **Show the query before executing** — Let the user review the `mongosh` command before running it.
