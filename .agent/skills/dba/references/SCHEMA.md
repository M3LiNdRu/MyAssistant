# MongoDB Collection Schemas

All field names are **camelCase** in MongoDB due to the `CamelCaseElementNameConvention` registered in the driver. The `_id` field is an `ObjectId`.

## Collection: `Expenses`

Individual expense records.

| Field       | BSON Type  | Description                        |
|-------------|------------|------------------------------------|
| `_id`       | ObjectId   | Unique identifier                  |
| `timestamp` | Date       | When the expense occurred          |
| `category`  | String     | Category name                      |
| `name`      | String     | Expense description/label          |
| `amount`    | Decimal128 | Expense amount                     |
| `currency`  | String     | Currency code (e.g. "EUR")         |
| `tags`      | Array\<String\> | Associated tag names          |

**C# class**: `MyAssistant.Apis.Expenses.Api.Resources.Expenses.Expense`

## Collection: `Categories`

Expense category definitions.

| Field         | BSON Type | Description               |
|---------------|-----------|---------------------------|
| `_id`         | ObjectId  | Unique identifier         |
| `name`        | String    | Category name             |
| `description` | String    | Category description      |

**C# class**: `MyAssistant.Apis.Expenses.Api.Resources.Categories.Category`

## Collection: `Tags`

A single document containing all available tag names.

| Field   | BSON Type        | Description              |
|---------|------------------|--------------------------|
| `_id`   | ObjectId         | Unique identifier        |
| `names` | Array\<String\>  | List of all tag names    |

**C# class**: `MyAssistant.Apis.Expenses.Api.Resources.Tags.TagDocument`

> **Note**: There are two `TagDocument` classes in the codebase (under `Resources/Tags/` and `Resources/Expenses/`) with identical structure. Both map to the same `Tags` collection.
