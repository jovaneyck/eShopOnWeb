using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.UnitTests.Builders;
using NSubstitute;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services.BasketServiceTests;

public class AddItemToBasket
{
    private readonly string _buyerId = "Test buyerId";
    private readonly IRepository<Basket> _basketRepo = new InMemoryBasketRepository();
    private readonly IAppLogger<BasketService> _mockLogger = Substitute.For<IAppLogger<BasketService>>();

    [Fact]
    public async Task AddsNewItemToEmptyBasket()
    {
        var basketService = new BasketService(_basketRepo, _mockLogger);
        
        var result = await basketService.AddItemToBasket(_buyerId, 1, 1.50m);
        
        Assert.NotNull(result);
        var persistedBasket = await _basketRepo.FirstOrDefaultAsync(new BasketWithItemsSpecification(_buyerId), CancellationToken.None);
        Assert.NotNull(persistedBasket);
        Assert.Equal(_buyerId, persistedBasket.BuyerId);
        Assert.Equal(_buyerId, persistedBasket.BuyerId);
        Assert.Single(persistedBasket.Items);
        Assert.Equal(1, persistedBasket.Items.First().CatalogItemId);
        Assert.Equal(1.50m, persistedBasket.Items.First().UnitPrice);
        Assert.Equal(1, persistedBasket.Items.First().Quantity);
    }

    [Fact]
    public async Task IncreasesQuantityWhenAddingExistingItem()
    {
        var basketService = new BasketService(_basketRepo, _mockLogger);
        var basket = new BasketBuilder().WithBuyerId(_buyerId).Build();
        await _basketRepo.AddAsync(basket, CancellationToken.None);
        
        await basketService.AddItemToBasket(_buyerId, 1, 1.50m);
        
        var result = await basketService.AddItemToBasket(_buyerId, 1, 1.50m);
        Assert.Equal(1, await _basketRepo.CountAsync(TestContext.Current.CancellationToken));
        Basket persistedBasket = (await _basketRepo.FirstOrDefaultAsync(new BasketWithItemsSpecification(_buyerId), CancellationToken.None))!;
        Assert.NotNull(result);
        Assert.Single(persistedBasket.Items);
        Assert.Equal(2, persistedBasket.Items.First().Quantity);
    }
}
