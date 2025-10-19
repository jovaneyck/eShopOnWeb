We'll be implementing a new feature today "split basket". It splits a given basket into two baskets, the original basket that keeps all "cheap" items (basket items with unit price less than 100 dollars), all expensive basket items get transferred to a newly created basket if and when a split needs to happen.

I expect the Basket aggregate to have a new method that does most of the heavy lifting:
```csharp
public void Basket? Split(decimal threshold) {...}
```

I expect the BasketService to expose a new method:
```csharp
public async Task<Result<Basket>> Split(int basketId) {...}
```

Acceptance criteria/unit tests:
* Splitting an empty basket results in no change
* Splitting a basket with only cheap items results in no change
* Splitting a basket with only expensive items results in no change
* Splitting a basket with a mixture of cheap and expensive items results in 2 baskets. The original basket contains only the cheap items. A newly created basket  for the same buyerId is created that contains all the expensive items.