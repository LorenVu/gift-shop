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
        return await _dbSet.Include(p => p.Properties).Include(x => x.Images).FirstOrDefaultAsync(p => p.ID.Equals(id));
    }

    public IQueryable<Product> GetProductWithProperty(Expression<Func<Product, bool>> predicate)
    {
        return _dbSet.Where(predicate).Include(x => x.Properties).Include(x => x.Images);
    }

    public async Task<int> CreateProduct(Product product)
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

                if (product.Images.Any())
                {
                    await _dbContext.ProductImages.AddRangeAsync(product.Images);
                }

                result = await _dbContext.SaveChangesAsync();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductRepository|CreateProduct|Error: {ex.Message}");
            }
        }

        return result;
    }

    public async Task<int> UpdateProduct(Product product)
    {
        var result = 0;

        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var existingProduct = _dbContext.Products
                                       .Include(p => p.Properties)
                                       .Include(p => p.Images)
                                       .SingleOrDefault(p => p.ID.Equals(product.ID));

                // Handle related properties
                UpdateProperties(existingProduct, product.Properties.ToList());
                _dbContext.Entry(existingProduct).State = EntityState.Modified;

                result = await _dbContext.SaveChangesAsync();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductRepository|UpdateProduct|Error: {ex.Message}");
            }
        }

        return result;
    }

    private void UpdateProperties(Product existingProduct, List<Property> updatedProperties)
    {
        foreach (var updatedProperty in updatedProperties)
        {
            if (updatedProperty.ID == Guid.Empty)
            {
                updatedProperty.ID = Guid.NewGuid();
                existingProduct.Properties.Add(updatedProperty);
            }
            else
            {
                var existingProperty = existingProduct.Properties.SingleOrDefault(p => p.ID == updatedProperty.ID);
                if (existingProperty != null)
                {
                    existingProperty.Name = updatedProperty.Name;
                    existingProperty.Value = updatedProperty.Value;
                }
                else
                {
                }
            }
        }
    }
}
