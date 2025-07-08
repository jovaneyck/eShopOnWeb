using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using System.Reflection;

namespace Microsoft.eShopWeb.UnitTests.Builders;

public class BasketBuilder
{
    private string _buyerId = "testbuyerId@test.com";
    private int _basketId = 1;
    private readonly List<(int catalogItemId, decimal unitPrice, int quantity)> _items = new();

    public BasketBuilder WithBuyerId(string buyerId)
    {
        _buyerId = buyerId;
        return this;
    }

    public BasketBuilder WithId(int id)
    {
        _basketId = id;
        return this;
    }

    public BasketBuilder WithItem(int catalogItemId, decimal unitPrice, int quantity = 1)
    {
        return WithItems((catalogItemId, unitPrice, quantity));
    }

    public BasketBuilder WithItems(params (int catalogItemId, decimal unitPrice, int quantity)[] items)
    {
        foreach (var (catalogItemId, unitPrice, quantity) in items)
        {
            _items.Add((catalogItemId, unitPrice, quantity));
        }
        return this;
    }

    public BasketBuilder WithOneBasketItem()
    {
        return WithItem(2, 3.40m, 4);
    }

    public Basket Build()
    {
        var b = new Basket(_buyerId);
        SetId(b, _basketId);

        foreach (var i in _items)
        {
            b.AddItem(i.catalogItemId, i.unitPrice, i.quantity);
        }

        return b;
    }

    private void SetId(Basket b, int id)
    {
        var idProperty = typeof(Basket).BaseType?.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        idProperty?.SetValue(b, id);
    }
}
