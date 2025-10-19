# User Story: Split a Basket

As a shop clerk, I want to be able to "split a basket" so that customers can leave with part of their basket whenever they don't have enough money on hand.

When splitting a given basket into two baskets, the original basket that keeps all "cheap" items (basket items with unit price less than 100 dollars), all expensive basket items get transferred to a newly created basket if and when a split needs to happen.

# Acceptance criteria

* Splitting an empty basket results in no change
* Splitting a basket with only cheap items results in no change
* Splitting a basket with only expensive items results in no change
* Splitting a basket with a mixture of cheap and expensive items results in 2 baskets. The original basket contains only the cheap items. A newly created basket  for the same buyerId is created that contains all the expensive items.