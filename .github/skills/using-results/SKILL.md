---
name: using-results
description: Guide for correctly using Ardalis.Result types in this codebase. Use this skill whenever writing or reviewing code that returns Result<T>, checks result status, or handles NotFound/Success/Error outcomes.
---

# Using Ardalis.Result in eShopOnWeb

This project uses **Ardalis.Result**. The generic `Result<T>` and non-generic `Result` have different APIs. Mixing them up causes compile errors.

## Key API differences

### `Result` (non-generic)
Has convenience methods: `IsNotFound()`, `IsSuccess`, etc.

### `Result<T>` (generic)
**No** `IsNotFound()` method. Use the `Status` property instead:

```csharp
// ❌ Doesn't compile on Result<T>
result.IsNotFound()

// ✅ Correct
result.Status == ResultStatus.NotFound
result.IsSuccess   // this one does exist on Result<T>
```