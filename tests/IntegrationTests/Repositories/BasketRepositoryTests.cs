using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.UnitTests.Builders;
using Xunit;

namespace Microsoft.eShopWeb.IntegrationTests.Repositories;

public class BasketRepositoryTests
{
    private (DbContext, EfRepository<Basket> repository) BuildFreshRepository()
    {
        var context = new CatalogContext(new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase(databaseName: "TestCatalog")
            .Options);
        return (context, new EfRepository<Basket>(context));
    }

    [Fact]
    public async Task CanPersistAndReadAggregate()
    {
        var (writeContext, writeRepo) = BuildFreshRepository();
        var basket = new BasketBuilder().WithOneBasketItem().Build();
        await writeRepo.AddAsync(basket, TestContext.Current.CancellationToken);
        await writeContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        
        var (_, repo) = BuildFreshRepository();
        var retrieved = await repo.FirstOrDefaultAsync(
            new BasketWithItemsSpecification(basket.Id), 
            TestContext.Current.CancellationToken);
        
        Assert.NotNull(retrieved);
        Assert.Equal(basket.BuyerId, retrieved.BuyerId);
        Assert.Equal(basket.TotalItems, retrieved.TotalItems);
        
        Assert.Equal(basket.Items.Single().Id, retrieved.Items.Single().Id);
        Assert.Equal(basket.Items.Single().BasketId, retrieved.Items.Single().BasketId);
        Assert.Equal(basket.Items.Single().CatalogItemId, retrieved.Items.Single().CatalogItemId);
        Assert.Equal(basket.Items.Single().Quantity, retrieved.Items.Single().Quantity);
        Assert.Equal(basket.Items.Single().UnitPrice, retrieved.Items.Single().UnitPrice);
    }
}
