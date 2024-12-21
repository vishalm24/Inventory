using Inventory.Models;

namespace Inventory.IServices
{
    public interface ICategoryRepository
    {
        dynamic Index(int pg);
        bool AddCategory(Category category);
        dynamic Update(int? id);
        void Update(Category category);
        string Delete(int id);
    }
}
