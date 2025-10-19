---
applyTo: '**'
---

# Summary

Implement a new feature. Take care to follow existing coding standards. Always take a look at the reference implementation and follow its style and idioms.

# Visual marker

The visual marker for this instruction is: ✨

# Reference implementation

* Service: #file:BasketService.cs (lookup and orchestration, delegate all functionality to the aggregates)
* Aggregate: #file:Basket.cs (rich domain logic)
* Repository: #file:BasketRepository.cs
* Unit test: #file:AddItemToBasket.cs Write ONLY unit tests in the style of this file. These tests cover both the service and aggregate functionality and use an in memory repository and fluent test data builders for maximum speed and clarity.

# Process

* For each acceptance criteria, make sure to include a unit test.
* If any method signatures or aggregate specifications are provided, make sure to use the exact patterns provided.
* Implement the feature taking special care to correctly distribute functionality between service, repository and aggregate.
* The build command for this solution is VERY SLOW. Ensure all code compiles and unit tests pass by running the following command which runs faster:

```shell
dotnet test .\\tests\\UnitTests\\UnitTests.csproj --no-restore
```
