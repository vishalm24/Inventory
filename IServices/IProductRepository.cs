using Inventory.Models;

namespace Inventory.IServices
{
    public interface IProductRepository
    {
        dynamic Index(int pg);
        bool AddCategory(Product product);
        dynamic Update(int? id);
        void Update(Product product);
        string Delete(int id);
    }
}
