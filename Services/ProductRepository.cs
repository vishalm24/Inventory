using Inventory.Data;
using Inventory.IServices;
using Inventory.Models;
using Inventory.Models.ViewModel;

namespace Inventory.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationContext _context;
        public ProductRepository(ApplicationContext context)
        {
            _context = context;
        }
        public dynamic Index(int pg)
        {
            IndexData indexData = new IndexData();
            List<Product> products = _context.Products.ToList();
            const int pageSize = 10;
            if (pg < 1)
                pg = 1;
            int recsCount = products.Count;
            indexData.pager = new Pager(recsCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            indexData.data = (from p in _context.Products
                              join c in _context.Categories
                              on p.CategoryId equals c.CategoryId
                              select new ProductCategorySummaryViewModel
                              {
                                  ProductId = p.ProductId,
                                  ProductName = p.ProductName,
                                  CategoryId = c.CategoryId,
                                  CategoryName = c.CategoryName
                              }).Skip(resSkip).Take(indexData.pager.PageSize).ToList();
            return indexData;
        }

        public bool AddCategory(Product product)
        {
            bool isDuplicate = IsDuplicate(product);
            if (isDuplicate)
            {
                return true;
            }
            _context.Products.Add(product);
            _context.SaveChangesAsync();
            return false;
        }
        public bool IsDuplicate(Product product)
        {
            bool isDuplicate = _context.Products
                .Any(p => p.ProductName == product.ProductName);
            return isDuplicate;
        }
        public dynamic Update(int? id)
        {
            Product product = new Product();
            if (id != null && id != 0)
            {
                product = _context.Products.Find(id);
            }
            return product;
        }
        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public string Delete(int id)
        {
            if (id != 0)
            {
                var product = _context.Products.Find(id);
                if (product == null)
                {
                    return "Not Found";
                }
                else
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                    return "Success";
                }
            }
            else
            {
                return "Bad Request";
            }
        }
    }
}
