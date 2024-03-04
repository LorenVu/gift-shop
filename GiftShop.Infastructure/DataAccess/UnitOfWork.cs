using GiftShop.Infastructure.Data;
using GiftShop.Infastructure.Interfaces;

namespace GiftShop.Infastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IBrandRepository Brands { get; }

    public ICategoryRepository Categories { get; }

    public IProductRepository Products { get; }

    public IPropertyRepository Properties { get; }

    public UnitOfWork(ApplicationDbContext context, IBrandRepository brandRepository, ICategoryRepository categoryRepository, IProductRepository productRepository, IPropertyRepository propertyRepository)
    {
        _context = context;
        Brands = brandRepository;
        Categories = categoryRepository;
        Products = productRepository;
        Properties = propertyRepository;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }

    public void SaveChange()
        => _context.SaveChanges();

    public async Task<int> SaveChangeAsync()
        => await _context.SaveChangesAsync();
}
