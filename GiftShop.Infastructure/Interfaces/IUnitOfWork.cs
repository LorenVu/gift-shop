namespace GiftShop.Infastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBrandRepository Brands { get; }
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }
    IPropertyRepository Properties { get; }
    void SaveChange();
    Task<int> SaveChangeAsync();
}
