using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;

namespace Microsoft.eShopWeb.Infrastructure.Data;

public class BasketRepository : EfRepository<Basket>
{
    private readonly CatalogContext context;
    
    public BasketRepository(CatalogContext dbContext) : base(dbContext)
    {
        context = dbContext;
    }

    public Task<Basket?> GetAggregateById(int id, CancellationToken cancellationToken)
    {
        return context.Baskets
            .Include(b=>b.Items)
            .SingleOrDefaultAsync(b=>b.Id == id, cancellationToken: cancellationToken);
    }
    
}
