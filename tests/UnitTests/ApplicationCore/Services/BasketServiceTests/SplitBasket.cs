using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.UnitTests.Builders;
using NSubstitute;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services.BasketServiceTests;

public class SplitBasket
{
    private readonly string _buyerId = "Test buyerId";
    private readonly IRepository<Basket> _basketRepo = new InMemoryBasketRepository();
    private readonly IAppLogger<BasketService> _mockLogger = Substitute.For<IAppLogger<BasketService>>();

    [Fact]
    public async Task SplittingEmptyBasketResultsInNoChange()
    {
        var basketService = new BasketService(_basketRepo, _mockLogger);
        var basket = new BasketBuilder().WithBuyerId(_buyerId).WithId(1).Build();
        await _basketRepo.AddAsync(basket, CancellationToken.None);

        var result = await basketService.Split(1);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal(1, await _basketRepo.CountAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task SplittingBasketWithOnlyCheapItemsResultsInNoChange()
    {
        var basketService = new BasketService(_basketRepo, _mockLogger);
        var basket = new BasketBuilder()
            .WithBuyerId(_buyerId)
            .WithId(1)
            .WithItems((1, 50m, 2), (2, 75m, 1), (3, 99.99m, 3))
            .Build();
        await _basketRepo.AddAsync(basket, CancellationToken.None);

        var result = await basketService.Split(1);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal(1, await _basketRepo.CountAsync(TestContext.Current.CancellationToken));
        var persistedBasket = await _basketRepo.GetByIdAsync(1, CancellationToken.None);
        Assert.NotNull(persistedBasket);
        Assert.Equal(3, persistedBasket.Items.Count);
    }

    [Fact]
    public async Task SplittingBasketWithOnlyExpensiveItemsResultsInNoChange()
    {
        var basketService = new BasketService(_basketRepo, _mockLogger);
        var basket = new BasketBuilder()
            .WithBuyerId(_buyerId)
            .WithId(1)
            .WithItems((1, 100m, 2), (2, 150m, 1), (3, 200m, 3))
            .Build();
        await _basketRepo.AddAsync(basket, CancellationToken.None);

        var result = await basketService.Split(1);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal(1, await _basketRepo.CountAsync(TestContext.Current.CancellationToken));
        var persistedBasket = await _basketRepo.GetByIdAsync(1, CancellationToken.None);
        Assert.NotNull(persistedBasket);
        Assert.Equal(3, persistedBasket.Items.Count);
    }

    [Fact]
    public async Task SplittingBasketWithMixedItemsResultsInTwoBaskets()
    {
        var basketService = new BasketService(_basketRepo, _mockLogger);
        var basket = new BasketBuilder()
            .WithBuyerId(_buyerId)
            .WithId(1)
            .WithItems((1, 50m, 2), (2, 150m, 1), (3, 75m, 3), (4, 200m, 1))
            .Build();
        await _basketRepo.AddAsync(basket, CancellationToken.None);

        var result = await basketService.Split(1);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, await _basketRepo.CountAsync(TestContext.Current.CancellationToken));

        var originalBasket = await _basketRepo.GetByIdAsync(1, CancellationToken.None);
        Assert.NotNull(originalBasket);
        Assert.Equal(_buyerId, originalBasket.BuyerId);
        Assert.Equal(2, originalBasket.Items.Count);
        Assert.All(originalBasket.Items, item => Assert.True(item.UnitPrice < 100m));

        var newBasket = result.Value;
        Assert.Equal(_buyerId, newBasket.BuyerId);
        Assert.Equal(2, newBasket.Items.Count);
        Assert.All(newBasket.Items, item => Assert.True(item.UnitPrice >= 100m));
    }
}
