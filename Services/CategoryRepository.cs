using Inventory.Data;
using Inventory.IServices;
using Inventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext _context;
        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public dynamic Index(int pg)
        {
            IndexData indexData = new IndexData();
            List<Category> categories = _context.Categories.ToList();
            const int pageSize = 10;
            if (pg < 1)
                pg = 1;
            int recsCount = categories.Count();
            indexData.pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            indexData.data = categories.Skip(recSkip).Take(indexData.pager.PageSize).ToList();
            return indexData;
        }

        public bool AddCategory(Category category)
        {
            bool isDuplicate = IsDuplicate(category);
            if (isDuplicate)
            {
                return true;
            }
            _context.Categories.AddAsync(category);
            _context.SaveChangesAsync();
            return false;
        }
        public bool IsDuplicate(Category category)
        {
            bool isDuplicate = _context.Categories
                .Any(p => p.CategoryName == category.CategoryName);
            return isDuplicate;
        }

        public dynamic Update(int? id)
        {
            Category category = new Category();
            if (id != null && id != 0)
            {
                category = _context.Categories.Find(id);
            }
            return category;
        }
        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public string Delete(int id) {
            if (id != 0)
            {
                bool status = _context.Products.Any(x => x.CategoryId == id);
                if (status)
                {
                    var category = _context.Categories.Find(id);
                    var products = _context.Products.Where(p => p.CategoryId == id).ToList();
                    _context.RemoveRange(products);
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                    return "Category with Products";
                }
                else
                {
                    var category = _context.Categories.Find(id);
                    if (category == null)
                    {
                        return "Not Found";
                    }
                    else
                    {
                        _context.Categories.Remove(category);
                        _context.SaveChanges();
                        return "Success";
                    }
                }
            }
            else
            {
                return "Bad Request";
            }
        }
    }
}
