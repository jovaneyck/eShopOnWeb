using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.UnitTests.Builders;
using NSubstitute;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Services.BasketServiceTests;

public class TransferBasket
{
    private readonly string _anonymousUserId = "anonymous-user-id";
    private readonly string _registeredUserId = "registered@microsoft.com";
    private readonly IRepository<Basket> _basketRepo = new InMemoryBasketRepository();
    private readonly IAppLogger<BasketService> _mockLogger = Substitute.For<IAppLogger<BasketService>>();

    [Fact]
    public async Task DoesNothingWhenAnonymousBasketNotExists()
    {
        var basketService = new BasketService(_basketRepo, _mockLogger);
        
        // Verify no anonymous basket exists
        var spec = new BasketWithItemsSpecification(_anonymousUserId);
        var anonymousBasket = await _basketRepo.FirstOrDefaultAsync(spec, TestContext.Current.CancellationToken);
        Assert.Null(anonymousBasket);
        
        // Transfer should do nothing
        await basketService.TransferBasketAsync(_anonymousUserId, _registeredUserId);
        
        // Verify no user basket was created
        var userSpec = new BasketWithItemsSpecification(_registeredUserId);
        var userBasket = await _basketRepo.FirstOrDefaultAsync(userSpec, TestContext.Current.CancellationToken);
        Assert.Null(userBasket);
    }

    [Fact]
    public async Task TransfersAnonymousBasketItemsToExistingUserBasket()
    {
        // Arrange - create anonymous basket with items
        var anonymousBasket = new BasketBuilder()
            .WithBuyerId(_anonymousUserId)
            .WithItem(1, 10m, 1)
            .WithItem(3,55m,7)
            .Build();
        
        await _basketRepo.AddAsync(anonymousBasket, TestContext.Current.CancellationToken);
        
        // Arrange - create existing user basket with items  
        var userBasket = new BasketBuilder()
            .WithBuyerId(_registeredUserId)
            .WithItem(1, 10m, 4) // Same item as anonymous basket
            .WithItem(2, 99m, 3)// Different item
            .Build(); 
        await _basketRepo.AddAsync(userBasket, TestContext.Current.CancellationToken);
        
        var basketService = new BasketService(_basketRepo, _mockLogger);
        
        // Act - transfer baskets
        await basketService.TransferBasketAsync(_anonymousUserId, _registeredUserId);
        
        // Assert - verify user basket has combined items
        var userSpec = new BasketWithItemsSpecification(_registeredUserId);
        var updatedUserBasket = await _basketRepo.FirstOrDefaultAsync(userSpec, TestContext.Current.CancellationToken);
        
        Assert.NotNull(updatedUserBasket);
        Assert.Equal(3, updatedUserBasket.Items.Count);
        Assert.Contains(updatedUserBasket.Items, x => x.CatalogItemId == 1 && x.UnitPrice == 10m && x.Quantity == 5);
        Assert.Contains(updatedUserBasket.Items, x => x.CatalogItemId == 2 && x.UnitPrice == 99m && x.Quantity == 3);
        Assert.Contains(updatedUserBasket.Items, x => x.CatalogItemId == 3 && x.UnitPrice == 55m && x.Quantity == 7);
        
        // Assert - verify anonymous basket was deleted
        var anonymousSpec = new BasketWithItemsSpecification(_anonymousUserId);
        var deletedAnonymousBasket = await _basketRepo.FirstOrDefaultAsync(anonymousSpec, TestContext.Current.CancellationToken);
        Assert.Null(deletedAnonymousBasket);
    }

    [Fact]
    public async Task CreatesNewUserBasketWhenNotExists()
    {
        // Arrange - create anonymous basket with items
        var anonymousBasket = new Basket(_anonymousUserId);
        anonymousBasket.AddItem(1, 15m, 2);
        anonymousBasket.AddItem(2, 25m, 1);
        await _basketRepo.AddAsync(anonymousBasket, TestContext.Current.CancellationToken);
        
        var basketService = new BasketService(_basketRepo, _mockLogger);
        
        // Act - transfer to non-existent user
        await basketService.TransferBasketAsync(_anonymousUserId, _registeredUserId);
        
        // Assert - verify new user basket was created with items
        var userSpec = new BasketWithItemsSpecification(_registeredUserId);
        var newUserBasket = await _basketRepo.FirstOrDefaultAsync(userSpec, TestContext.Current.CancellationToken);
        
        Assert.NotNull(newUserBasket);
        Assert.Equal(_registeredUserId, newUserBasket.BuyerId);
        Assert.Equal(2, newUserBasket.Items.Count);
        Assert.Contains(newUserBasket.Items, x => x.CatalogItemId == 1 && x.UnitPrice == 15m && x.Quantity == 2);
        Assert.Contains(newUserBasket.Items, x => x.CatalogItemId == 2 && x.UnitPrice == 25m && x.Quantity == 1);
        
        // Assert - verify anonymous basket was deleted
        var anonymousSpec = new BasketWithItemsSpecification(_anonymousUserId);
        var deletedAnonymousBasket = await _basketRepo.FirstOrDefaultAsync(anonymousSpec, TestContext.Current.CancellationToken);
        Assert.Null(deletedAnonymousBasket);
    }

    [Fact]
    public async Task TransfersEmptyAnonymousBasketCorrectly()
    {
        // Arrange - create empty anonymous basket
        var anonymousBasket = new Basket(_anonymousUserId);
        await _basketRepo.AddAsync(anonymousBasket, TestContext.Current.CancellationToken);
        
        // Arrange - create existing user basket with items
        var userBasket = new Basket(_registeredUserId);
        userBasket.AddItem(1, 10m, 2);
        await _basketRepo.AddAsync(userBasket, TestContext.Current.CancellationToken);
        
        var basketService = new BasketService(_basketRepo, _mockLogger);
        
        // Act - transfer empty anonymous basket
        await basketService.TransferBasketAsync(_anonymousUserId, _registeredUserId);
        
        // Assert - user basket items unchanged
        var userSpec = new BasketWithItemsSpecification(_registeredUserId);
        var updatedUserBasket = await _basketRepo.FirstOrDefaultAsync(userSpec, TestContext.Current.CancellationToken);
        
        Assert.NotNull(updatedUserBasket);
        Assert.Single(updatedUserBasket.Items);
        Assert.Contains(updatedUserBasket.Items, x => x.CatalogItemId == 1 && x.UnitPrice == 10m && x.Quantity == 2);
        
        // Assert - anonymous basket was still deleted
        var anonymousSpec = new BasketWithItemsSpecification(_anonymousUserId);
        var deletedAnonymousBasket = await _basketRepo.FirstOrDefaultAsync(anonymousSpec, TestContext.Current.CancellationToken);
        Assert.Null(deletedAnonymousBasket);
    }
}
