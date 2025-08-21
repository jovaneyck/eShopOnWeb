using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.UnitTests.Builders;
using NSubstitute;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services.BasketServiceTests;

public class DeleteBasket
{
    private readonly string _buyerId = "Test buyerId";
    private readonly IRepository<Basket> _basketRepo = new InMemoryBasketRepository();
    private readonly IAppLogger<BasketService> _mockLogger = Substitute.For<IAppLogger<BasketService>>();

    [Fact]
    public async Task DeletesBasketSuccessfully()
    {
        // Arrange - create and add a basket to the repository
        var basket = new BasketBuilder()
            .WithBuyerId(_buyerId)
            .WithItems(
                (1,1.1m,1),
                (2,1.1m,1))
            .Build();
        await _basketRepo.AddAsync(basket, TestContext.Current.CancellationToken);
        
        var basketService = new BasketService(_basketRepo, _mockLogger);

        // Act - delete the basket
        await basketService.DeleteBasketAsync(basket.Id);

        // Assert - verify basket is deleted
        var deletedBasket = await _basketRepo.GetByIdAsync(basket.Id, TestContext.Current.CancellationToken);
        Assert.Null(deletedBasket);
    }

    [Fact]
    public async Task ThrowsWhenDeletingNonExistentBasket()
    {
        var basketService = new BasketService(_basketRepo, _mockLogger);

        // Act & Assert - should throw when trying to delete non-existent basket
        await Assert.ThrowsAsync<ArgumentNullException>(() => basketService.DeleteBasketAsync(999));
    }
}
