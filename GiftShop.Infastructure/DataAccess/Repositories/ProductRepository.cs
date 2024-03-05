using GiftShop.Domain.Entities;
using GiftShop.Infastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GiftShop.Infastructure.DataAccess.Repositories;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    readonly ILogger<ProductRepository> _logger;

    public ProductRepository(ApplicationDbContext dataContext, ILogger<ProductRepository> logger) : base(dataContext)
    {
        _logger = logger;
    }

    public async Task<Product> FindProductByIDWithProperty(Guid id)
    {
        return await _dbSet.Include(p => p.Properties).FirstOrDefaultAsync(p => p.ID.Equals(id));
    }

    public IQueryable<Product> GetProductWithProperty(Expression<Func<Product, bool>> predicate)
    {
        return _dbSet.Where(predicate).Include(x => x.Properties);
    }

    public async Task<int> CreateProductWithProperty(Product product)
    {
        var result = 0;

        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                await _dbContext.Products.AddAsync(product);

                if (product.Properties.Any())
                {
                    await _dbContext.ProductProperties.AddRangeAsync(product.Properties);
                }

                result = await _dbContext.SaveChangesAsync();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductRepository|CreateProductWithProperty|Error: {ex.Message}");
            }
        }

        return result;
    }
}
