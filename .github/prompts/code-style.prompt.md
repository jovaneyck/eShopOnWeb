# Reference implementation

Always follow the structure and coding style of the reference implementation.

* Service: [BasketService](..\..\src\ApplicationCore\Services\BasketService.cs) (covers lookup and orchestration, delegate all functionality to the aggregates)
* Aggregate: [BasketService](..\..\src\ApplicationCore\Entities\BasketAggregate\Basket.cs) (covers rich domain logic)
* Repository: [IRepository](..\..\src\ApplicationCore\Interfaces\IRepository.cs) and[EfRepository](..\..\src\Infrastructure\Data\EfRepository.cs) (covers data access and persistence)
* Unit tests: [AddItemToBasket.cs](..\..\tests\UnitTests\ApplicationCore\Services\BasketServiceTests/AddItemToBasket.cs) Write ONLY unit tests in the style of this file. Unit tests cover both the service and aggregate functionality and use an in memory repository and fluent test data builders for maximum speed and clarity.