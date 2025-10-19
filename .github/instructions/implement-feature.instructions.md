---
applyTo: '**'
---

# Summary

Implement a new feature. Take care to follow existing coding standards. Always take a look at the reference implementation and follow its style and idioms.

# Reference implementation

* Service: #file:BasketService.cs (lookup and orchestration, delegate all functionality to the aggregates)
* Aggregate: #file:Basket.cs (rich domain logic)
* Repository: #file:BasketRepository.cs
* Unit test: #file:AddItemToBasket.cs Write ONLY unit tests in the style of this file. These tests cover both the service and aggregate functionality and use an in memory repository and fluent test data builders for maximum speed and clarity.

# Process

* Write a unit test for every acceptance criteria.
* Implement the feature, correctly distributing functionality between service, repository and aggregate.
* The build for this solution is VERY SLOW. Ensure all code compiles and unit tests pass by running the following command:

```shell
dotnet test .\\tests\\UnitTests\\UnitTests.csproj --no-restore
```