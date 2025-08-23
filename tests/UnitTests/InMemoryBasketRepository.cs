using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.UnitTests;

public class InMemoryBasketRepository : IRepository<Basket>
{
    private readonly List<Basket> _baskets = new();

    public Task<Basket> AddAsync(Basket entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == 0)
        {
            var nextId = _baskets.Max(b => b.Id) + 1;
            var idProperty = typeof(Basket).BaseType!.GetProperty("Id");
            idProperty!.SetValue(entity, nextId);
        }
        _baskets.Add(entity);
        return Task.FromResult(entity);
    }

    public async Task<IEnumerable<Basket>> AddRangeAsync(IEnumerable<Basket> entities, CancellationToken cancellationToken = default)
    {
        var es = entities.ToArray();
        foreach (var entity in es)
        {
            await this.AddAsync(entity, cancellationToken);
        }
        return es;
    }

    public Task<int> UpdateAsync(Basket entity, CancellationToken cancellationToken = default)
    {
        var existing = _baskets.FirstOrDefault(b => b.Id == entity.Id);
        return Task.FromResult(existing != null ? 1 : 0);
    }

    public async Task<int> UpdateRangeAsync(IEnumerable<Basket> entities, CancellationToken cancellationToken = default)
    {
        int count = 0;
        foreach (var entity in entities)
        {
            count += await UpdateAsync(entity, cancellationToken);
        }
        return count;
    }

    public Task<int> DeleteAsync(Basket entity, CancellationToken cancellationToken = default)
    {
        var removed = _baskets.Remove(entity);
        return Task.FromResult(removed ? 1 : 0);
    }

    public async Task<int> DeleteRangeAsync(IEnumerable<Basket> entities, CancellationToken cancellationToken = default)
    {
        int count = 0;
        foreach (var entity in entities)
        {
            count += await DeleteAsync(entity, cancellationToken);
        }
        return count;
    }

    public async Task<int> DeleteRangeAsync(ISpecification<Basket> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        var toDelete = query.ToList();
        return await DeleteRangeAsync(toDelete, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(0);
    }

    public Task<Basket?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        var basket = _baskets.FirstOrDefault(b => b.Id.Equals(id));
        return Task.FromResult(basket);
    }

    public Task<Basket?> GetBySpecAsync<TSpec>(TSpec specification, CancellationToken cancellationToken = default) where TSpec : ISpecification<Basket>, ISingleResultSpecification<Basket>
    {
        var query = ApplySpecification(specification);
        return Task.FromResult(query.FirstOrDefault());
    }

    public Task<TResult?> GetBySpecAsync<TResult>(ISpecification<Basket, TResult> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        return Task.FromResult(query.FirstOrDefault());
    }

    public Task<Basket?> FirstOrDefaultAsync(ISpecification<Basket> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        return Task.FromResult(query.FirstOrDefault());
    }

    public Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<Basket, TResult> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        return Task.FromResult(query.FirstOrDefault());
    }

    public Task<Basket?> SingleOrDefaultAsync(ISingleResultSpecification<Basket> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        return Task.FromResult(query.SingleOrDefault());
    }

    public Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<Basket, TResult> specification, CancellationToken cancellationToken = default)
    {
        // Simple implementation - return default for now
        return Task.FromResult(default(TResult));
    }

    public Task<List<Basket>> ListAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_baskets.ToList());
    }

    public Task<List<Basket>> ListAsync(ISpecification<Basket> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        return Task.FromResult(query.ToList());
    }

    public Task<List<TResult>> ListAsync<TResult>(ISpecification<Basket, TResult> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        return Task.FromResult(query.ToList());
    }

    public Task<int> CountAsync(ISpecification<Basket> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        return Task.FromResult(query.Count());
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_baskets.Count);
    }

    public Task<bool> AnyAsync(ISpecification<Basket> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        return Task.FromResult(query.Any());
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_baskets.Any());
    }

    public async IAsyncEnumerable<Basket> AsAsyncEnumerable(ISpecification<Basket> specification)
    {
        var query = ApplySpecification(specification);
        foreach (var item in query)
        {
            yield return item;
        }
        await Task.CompletedTask;
    }

    private IQueryable<Basket> ApplySpecification<TSpec>(TSpec specification) where TSpec : ISpecification<Basket>
    {
        return _baskets.Where(specification.IsSatisfiedBy).AsQueryable();
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<Basket, TResult> specification)
    {
        return specification.Evaluate(_baskets.Where(specification.IsSatisfiedBy)).AsQueryable();
    }
}
