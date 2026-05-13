using System.Collections.Generic;
using System.Linq;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;

public class Basket : BaseEntity, IAggregateRoot
{
    public string BuyerId { get; private set; }
    private readonly List<BasketItem> _items = new List<BasketItem>();
    public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

    public int TotalItems => _items.Sum(i => i.Quantity);


    public Basket(string buyerId)
    {
        BuyerId = buyerId;
    }

    public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
    {
        if (!Items.Any(i => i.CatalogItemId == catalogItemId))
        {
            _items.Add(new BasketItem(catalogItemId, quantity, unitPrice));
            return;
        }
        var existingItem = Items.First(i => i.CatalogItemId == catalogItemId);
        existingItem.AddQuantity(quantity);
    }

    public void RemoveEmptyItems()
    {
        _items.RemoveAll(i => i.Quantity == 0);
    }

    public void SetNewBuyerId(string buyerId)
    {
        BuyerId = buyerId;
    }

    public Basket? Split(decimal threshold)
    {
        var expensiveItems = _items.Where(i => i.UnitPrice >= threshold).ToList();

        if (expensiveItems.Count == 0 || expensiveItems.Count == _items.Count)
        {
            return null;
        }

        var newBasket = new Basket(BuyerId);
        foreach (var item in expensiveItems)
        {
            newBasket.AddItem(item.CatalogItemId, item.UnitPrice, item.Quantity);
            _items.Remove(item);
        }

        return newBasket;
    }
}
